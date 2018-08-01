namespace Server

open System
open FSharp.Core
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Firebase
open Fable.Import.Firebase.Functions
open Fable.PowerPack


module App =
    admin.initializeApp() |> ignore

    [<Pojo>]
    type CorsOption = {
        origin: obj
        credentials: bool
    }
    type Cors = CorsOption -> express.RequestHandler
    type CookieParser = unit -> express.RequestHandler
    let [<Fable.Core.Import("default", "cors")>] cors: Cors = jsNative
    let [<Fable.Core.Import("default", "cookie-parser")>] cookieParser: CookieParser = jsNative

    [<Emit("$1[$0]")>]
    let get<'a> (key: string) (x: obj) : 'a = jsNative

    let makeMap (o: obj) : Map<string, string> option =
        try
            [ for key in JS.Object.keys o -> 
                key, get<string> key o ]
            |> Map.ofList
            |> Some
        with
        | x ->
            None

    let validateFirebaseIdToken (req: express.Request) (res: express.Response) (next: obj -> unit) =
        let authHeader =
            req.headers 
            |> makeMap
            |> Option.bind (Map.tryFind "authorization")
            |> Option.filter (fun h -> h.StartsWith("Bearer "))
            |> Option.map (fun h -> h.Split([|' '|]).[1])

        let cookieSession =
            req.cookies
            |> (fun c -> JS.console.log ("Cookie passed through", req.cookies); c)
            |> makeMap
            |> Option.bind (Map.tryFind "__session")

        JS.console.log("Header and cookie", authHeader, cookieSession)
        let idToken =
            match authHeader, cookieSession with
            | Some idToken, _ -> Some idToken
            | _, Some idToken -> Some idToken
            | _, _ -> None

        let unauthorized (res: express.Response) =
            res.status(403).send("Unauthorized") |> ignore
            () :> obj

        let authenticate (req: express.Request) decodedIdToken =
            req.user <- decodedIdToken
            next() :> obj

        match idToken with
        | None ->
            res.status(403).send("Unauthorized") |> ignore
            () :> obj
        | Some idToken ->
            let verifyIdTokenPromise = Fable.Import.Firebase.admin.auth().verifyIdToken(idToken)
            JS.console.info("Verifying")
            verifyIdTokenPromise
            |> Promise.either (fun token -> !^(authenticate req token)) (fun token -> !^(unauthorized res))
            |> Promise.start
            :> obj

    let validateFirebaseIdToken' = new System.Func<express.Request, express.Response, obj -> unit, obj>(validateFirebaseIdToken)

    let expressApp = express.Invoke()

    expressApp.``use``(cookieParser()) |> ignore
    expressApp.``use``(cors({origin = "http://localhost:5000"; credentials = true})) |> ignore
    expressApp.``use``(validateFirebaseIdToken) |> ignore
    expressApp.get
        (U2.Case1 "/hello", 
            fun (req:express.Request) (res:express.Response) _ ->
                res.send(sprintf "Hello %O" req.user) |> box)
    |> ignore

    let app = https.onRequest expressApp
