namespace Fable.Import.Firebase.Functions
open Fable.Import
open Fable.Import.JS
open Fable.Core

[<AutoOpen>]
module Https =

    type [<AllowNullLiteral>] HttpsFunction =
        interface end

    type [<AllowNullLiteral>] IExports =
        abstract onRequest: handler: (express.Request -> express.Response -> unit) -> HttpsFunction

    let [<Fable.Core.Import("https", "firebase-functions")>] https: IExports = jsNative
