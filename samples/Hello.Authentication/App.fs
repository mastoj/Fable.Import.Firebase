namespace Server



open System
open FSharp.Core
open Fable.Core
open Fable.Import
open Fable.Import.Firebase
open Fable.Import.Firebase.Functions
open Fable.PowerPack
open Fable.Core.JsInterop

module App =
    module private ExpressHelpers =
        let toRequestHandler handler =
            new System.Func<express.Request, express.Response, (obj -> unit), obj>(handler)

        let returnJson (res: express.Response) x =
            res.setHeader("Content-Type", U2.Case1 "application/json")
            res.json(x) |> box

        let getPath path handler (app: express.Express) =
            let requestHandler = handler |> toRequestHandler
            app.get(U2.Case1 "/hello", requestHandler) |> ignore
            app

        let inline useMiddleware (x) (app: express.Express) =
            app.``use``([|x|]) |> ignore
            app

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

    [<Pojo>]
    type Hello = {
        FirstName: string
        LastName: string
    }

    let handler (req:express.Request) (res:express.Response) _ =
        let user = req.user
        let name: obj = user?name |> unbox
        let hello = {
            FirstName = (name :?> string)
            LastName = "Doe"
        }
        ExpressHelpers.returnJson res hello

    let expressApp = express.Invoke()
    expressApp
    |> ExpressHelpers.useMiddleware (cookieParser())
    |> ExpressHelpers.useMiddleware (cors({origin = "http://localhost:5000"; credentials = true}))
    |> ExpressHelpers.useMiddleware (validateFirebaseIdToken |> ExpressHelpers.toRequestHandler)
    |> ExpressHelpers.getPath "/hello" handler
    |> ignore

    let app = https.onRequest expressApp
