module GiraffeExample.TodoStore

open System
open System.Collections.Concurrent

type TodoId = Guid

type NewTodo = { Description: string }

type Todo =
    { Id: TodoId
      Description: string
      Created: DateTime
      IsCompleted: bool }

type TodoStore() =
    let data = ConcurrentDictionary<TodoId, Todo>()

    let get id =
        let (success, value) = data.TryGetValue(id)
        if success then Some value else None

    member _.Create(todo) = data.TryAdd(todo.Id, todo)

    member _.Update(todo) =
        data.TryUpdate(todo.Id, todo, data[todo.Id])

    member _.Delete(id) = data.TryRemove id
    member _.Get(id) = get id
    member _.GetAll() = data.Values |> Seq.toArray
