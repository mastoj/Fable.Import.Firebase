namespace Server

open Fable.Core
open Fable.Core.JsInterop

module App =
    open Fable.Import.Firebase.Functions
    open Fable.Import

    let app = express.Invoke()
    app.get
        (U2.Case1 "/hello/:name", 
            fun (req:express.Request) (res:express.Response) _ ->
                res.send(sprintf "Hello %O" req.``params``?name) |> box)
    |> ignore

    let helloWorld = https.onRequest app