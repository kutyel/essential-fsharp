open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Http
open Giraffe
open Giraffe.EndpointRouting
open Giraffe.ViewEngine
open GiraffeExample.Todos
open GiraffeExample.TodoStore

let indexView =
    html [] [
        head [] [
            title [] [ str "Giraffe Example" ]
        ]
        body [] [
            h1 [] [ str "I |> F#" ]
            p [ _class "some-css-class"
                _id "someId" ] [
                str "Hello World from the Giraffe View Engine"
            ]
        ]
    ]

let sayHelloNameHandler (name: string) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        {| Response = $"Hello {name}, how are you ?" |}
        |> ctx.WriteJsonAsync

let apiRoutes =
    [ GET [ route "/" (json {| Response = "Hello from 🦒!!" |})
            routef "/%s" sayHelloNameHandler ] ]


let endpoints =
    [ GET [ route "/" (htmlView indexView) ]
      subRoute "/api" apiRoutes
      subRoute "/api/todo" apiTodoRoutes ]

let notFoundHandler = "Not Found" |> text |> RequestErrors.notFound

let configureApp (appBuilder: IApplicationBuilder) =
    appBuilder
        .UseRouting()
        .UseGiraffe(endpoints)
        .UseGiraffe(notFoundHandler)

let configureServices (services: IServiceCollection) =
    services
        .AddRouting()
        .AddGiraffe()
        .AddSingleton<TodoStore>(TodoStore())
    |> ignore

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    configureServices builder.Services

    let app = builder.Build()

    if app.Environment.IsDevelopment() then
        app.UseDeveloperExceptionPage() |> ignore

    configureApp app
    app.Run()

    0 // Exit code
