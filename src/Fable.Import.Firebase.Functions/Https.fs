module Fable.Import.Firebase.Functions.Https
open Fable.Import
open Fable.Core

type [<AllowNullLiteral>] HttpsFunction =
    interface end

type [<AllowNullLiteral>] IExports =
    abstract onRequest: handler: (express.Request -> express.Response -> unit) -> HttpsFunction

let [<Fable.Core.Import("https", "firebase-functions")>] https: Https.IExports = jsNative