module GiraffeExample.Todos

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Http
open Giraffe
open Giraffe.EndpointRouting
open GiraffeExample.TodoStore

module Handlers =
    let viewTodosHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            let store = ctx.GetService<TodoStore>()
            store.GetAll() |> ctx.WriteJsonAsync

    let viewTodoHandler (id: Guid) =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let store = ctx.GetService<TodoStore>()

                return!
                    (match store.Get(id) with
                     | Some todo -> json todo
                     | None -> RequestErrors.NOT_FOUND "Not Found")
                        next
                        ctx
            }

    let createTodoHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! newTodo = ctx.BindJsonAsync<NewTodo>()
                let store = ctx.GetService<TodoStore>()

                let created =
                    { Id = Guid.NewGuid()
                      Description = newTodo.Description
                      Created = DateTime.UtcNow
                      IsCompleted = false }
                    |> store.Create

                return! json created next ctx
            }

    let updateTodoHandler (id: Guid) =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! todo = ctx.BindJsonAsync<Todo>()
                let store = ctx.GetService<TodoStore>()

                return!
                    (match store.Update(todo) with
                     | true -> json true
                     | false -> RequestErrors.GONE "Gone")
                        next
                        ctx
            }

    let deleteTodoHandler (id: Guid) =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let store = ctx.GetService<TodoStore>()

                return!
                    (match store.Get(id) with
                     | Some existing ->
                         let deleted = store.Delete(KeyValuePair(id, existing))
                         json deleted
                     | None -> RequestErrors.GONE "Gone")
                        next
                        ctx
            }


let apiTodoRoutes =
    [ GET [ routef "/%O" Handlers.viewTodoHandler
            route "" Handlers.viewTodosHandler ]
      POST [ route "" Handlers.createTodoHandler ]
      PUT [ routef "/%O" Handlers.updateTodoHandler ]
      DELETE [ routef "/%O" Handlers.deleteTodoHandler ] ]

module Data =
    let private create description isCompleted =
        { Id = Guid.NewGuid()
          Description = description
          Created = DateTime.UtcNow
          IsCompleted = isCompleted }

    let todoList =
        [ ("Hit the gym", false)
          ("Pay bills", true)
          ("Meet George", false)
          ("Buy eggs", false)
          ("Read a book", true)
          ("Read Essential F#", false) ]
        |> List.map (fun (todo, isCompleted) -> create todo isCompleted)

module Views =
    open Giraffe.ViewEngine

    let private showListItem (todo: Todo) =
        let style =
            if todo.IsCompleted then
                [ _class "checked" ]
            else
                []

        li style [ str todo.Description ]

    let todoView items =
        [ div [ _id "myDIV"; _class "header" ] [
              h2 [] [ str "My To Do List" ]
              input [ _type "text"
                      _id "myInput"
                      _placeholder "Title..." ]
              span [ _class "addBtn"
                     _onclick "newElement()" ] [
                  str "Add"
              ]
          ]
          ul [ _id "myUL" ] [
              for todo in items do
                  showListItem todo
          ]
          script [ _src "js/main.js"
                   _type "text/javascript" ] [] ]
        |> Shared.masterPage "My ToDo App"
