// Generated by RouteProvider 0.0.0.0
namespace MyNamespace

open System
module MyModule =
  let getProjectComments (project:int64) (comment:int64) =
      "projects/" + project.ToString() + "comments/" + comment.ToString()
  let putProjectComments (project:int64) =
      "projects/" + project.ToString() + "comments/"

  module Internal =
    let fakeBaseUri = new Uri("http://a.a")

    exception RouteNotMatchedException of string * string

  type MyRoutes<'TContext, 'TReturn> =
    { getProjectComments: 'TContext->int64->int64->'TReturn
      putProjectComments: 'TContext->int64->'TReturn
      notFound: ('TContext->string->string->'TReturn) option }

    member inline private this.HandleNotFound(context, verb, path) =
      match this.notFound with
      | None -> raise (Internal.RouteNotMatchedException (verb, path))
      | Some(notFound) -> notFound context verb path

    member this.DispatchRoute(context:'TContext, verb:string, path:string) : 'TReturn =
      let parts = path.Split('/')
      let start = if parts.[0] = "" then 1 else 0
      let endOffset = if parts.Length > 0 && parts.[parts.Length - 1] = "" then 1 else 0
      match parts.Length - start - endOffset with
      | 4 ->
        if String.Equals(parts.[0 + start],"projects") then
          let mutable project = 0L
          if Int64.TryParse(parts.[1 + start], &project) then
            if String.Equals(parts.[2 + start],"comments") then
              let mutable comment = 0L
              if Int64.TryParse(parts.[3 + start], &comment) then
                if verb = "GET" then this.getProjectComments context project comment
                else this.HandleNotFound(context, verb, path)
              else this.HandleNotFound(context, verb, path)
            else this.HandleNotFound(context, verb, path)
          else this.HandleNotFound(context, verb, path)
        else this.HandleNotFound(context, verb, path)
      | 3 ->
        if String.Equals(parts.[0 + start],"projects") then
          let mutable project = 0L
          if Int64.TryParse(parts.[1 + start], &project) then
            if String.Equals(parts.[2 + start],"comments") then
              if verb = "POST" then this.putProjectComments context project
              else this.HandleNotFound(context, verb, path)
            else this.HandleNotFound(context, verb, path)
          else this.HandleNotFound(context, verb, path)
        else this.HandleNotFound(context, verb, path)
      | _ ->
        this.HandleNotFound(context, verb, path)

    member this.DispatchRoute(context:'TContext, verb:string, uri:Uri) : 'TReturn =
      // Ensure we have an Absolute Uri, or just about every method on Uri chokes
      let uri = if uri.IsAbsoluteUri then uri else new Uri(Internal.fakeBaseUri, uri)
      let path = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped)
      this.DispatchRoute(context, verb, path)

    static member Router(getProjectComments: 'TContext->int64->int64->'TReturn,
                         putProjectComments: 'TContext->int64->'TReturn,
                         ?notFound: 'TContext->string->string->'TReturn) : MyRoutes<_,_> =
      { getProjectComments = getProjectComments
        putProjectComments = putProjectComments
        notFound = notFound}