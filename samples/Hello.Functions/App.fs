namespace Server

open Fable.Core

module App =
    open Fable.Import.Firebase.Functions.Https
    open Fable.Import.express

    let private handler req (res: Response) = res.send("Hello world") |> ignore

    let helloWorld = https.onRequest handler