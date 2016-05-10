open System
open System.IO
open Owin
open Microsoft.Owin
open System.Threading.Tasks
open Microsoft.Owin.Hosting
open Microsoft.Owin.Host.HttpListener

type WebApp() =
  let handleOwinContext (ctx:IOwinContext) =
    try
      ctx.Response.Write "HI"
    with
    | MyNamespace.MyModule.Internal.RouteNotMatchedException(verb, path) ->
      printfn "Route %s %s not matched" verb path

  let owinHandler = fun (context:IOwinContext) (_:Func<Task>) ->
    handleOwinContext context;
    Task.FromResult(null) :> Task;

  member x.Configuration (app:IAppBuilder) =
    app.Use(owinHandler) |> ignore

[<assembly: OwinStartup(typeof<WebApp>)>]
do ()

[<EntryPoint>]
let main argv =
    let url = "http://*:4567"

    try
      printfn "Starting up..."
      use app = WebApp.Start<WebApp>(url)
      printfn "Listening on %s" url
      Console.ReadLine() |> ignore
      0
    with
      | :? System.Net.HttpListenerException as ex ->
        let cmdStr = @"netsh http add urlacl url=""http://*:4567/"" delegate=yes user=everyone"
        printfn "Access denied. Run this command in an admin elevated terminal: %A" cmdStr
        1
