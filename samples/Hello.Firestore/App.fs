namespace Server

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Firebase
open Fable.PowerPack

module App =

    [<Pojo>]
    type Wat = {
        Name: string
        Other: int
    }

    let app = admin.initializeApp()
    let db = app.firestore()

    let ok x = printfn "OK %A" x
    let error x = printfn "ERROR %A" x

    let doc = db.doc("test/wat2")
    doc.set({Name = "Tomas Jansson"; Other = 36})
    |> Promise.either (fun x -> !^(ok x)) (fun x -> !^(error x))
    |> Promise.start
    |> ignore
