module Fable.Import.Firebase.Functions.Https
open Fable.Import
open Fable.Core

type [<AllowNullLiteral>] IHttpsFunction =
    interface end

type [<AllowNullLiteral>] IExports =
    abstract onRequest: handler: (express.Request -> express.Response -> unit) -> IHttpsFunction

let [<Fable.Core.Import("https", "firebase-functions")>] https: IExports = jsNative
