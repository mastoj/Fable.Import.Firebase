namespace Fable.Import.Firebase.Functions
open Fable.Import
open Fable.Core

[<AutoOpen>]
module Https =

    type [<AllowNullLiteral>] IHttpsFunction =
        interface end

    type [<AllowNullLiteral>] IExports =
        abstract onRequest: handler: (express.Request -> express.Response -> unit) -> IHttpsFunction

    let [<Fable.Core.Import("https", "firebase-functions")>] https: IExports = jsNative
