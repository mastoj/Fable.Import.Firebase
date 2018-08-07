namespace Server

open FSharp.Core
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Firebase
open Fable.PowerPack

module App =

    [<Pojo>]
    type Wat = {
        Name: string
        Other: int
        Timestamp: System.DateTime
    }

    let options = Admin.AppOptions.defaultOptions()

    let credentials = Admin.credential.cert(U2.Case1("/Users/tomasjansson/credentials/uc-test-tomas-firebase-adminsdk-om5at-706c53e692.json"))
    let appOptions = options
    appOptions.credential <- Some (credentials)
    appOptions.databaseURL <- Some "https://uc-test-tomas.firebaseio.com"
    let app = admin.initializeApp(appOptions)
    let db = app.firestore()

    let ok x = printfn "OK %A" x
    let error x = printfn "ERROR %A" x

    let doc = db.doc("test/wat3")
    doc.set({Name = "Tomas Jansson"; Other = 36; Timestamp = (System.DateTime.Now)})
    |> Promise.either (fun x -> !^(ok x)) (fun x -> !^(error x))
    |> Promise.start
    |> ignore
