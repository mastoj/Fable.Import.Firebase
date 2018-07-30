namespace Server

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Firebase.Functions
open Fable.Import.Firebase
open Fable.Import
open Fable.PowerPack


module App =

    [<Pojo>]
    type CorsOption = {
        origin: obj
    }
    type Cors = CorsOption -> express.RequestHandler
    type CookieParser = unit -> express.RequestHandler
    let [<Fable.Core.Import("default", "cors")>] cors: Cors = jsNative
    let [<Fable.Core.Import("default", "cookie-parser")>] cookieParser: CookieParser = jsNative

    let validateFirebaseIdToken (req: express.Request) (res: express.Response) (next: obj -> unit) =
        // let headers = req.headers?authorization |> isNull
        // let authorization = headers?authorization
        let headerKeys = (JS.Object.keys req.headers).ToArray()
        let authorization = 
            if Array.contains "authorization" headerKeys
            then
                let auth = req.headers?authorization
                let authString = ((auth()) :?> string)
                if authString.StartsWith("Bearer ")
                then Some (authString.Split([|' '|]).[1])
                else None
            else None

        let cookies =
            if Array.contains "cookies" headerKeys
            then
                let cookiesKeys = (JS.Object.keys req.cookies).ToArray()
                if Array.contains "__session" cookiesKeys
                then Some (req.cookies?__session() :?> string)
                else None
            else None

        let idToken =
            match authorization, cookies with
            | Some idToken, _ -> Some idToken
            | _, Some idToken -> Some idToken
            | _, _ -> None

        match idToken with
        | None ->
            res.status(403).send("Unauthorized") |> ignore
            () :> obj
        | Some idToken ->
            let verifyIdTokenPromise = Fable.Import.Firebase.admin.auth().verifyIdToken(idToken)
            verifyIdTokenPromise
            |> Promise.
            // verifyIdTokenPromise.th
            // let okToken = Func<string, 
//            admin.auth()
            () :> obj
//admin.auth().verifyIdToken(idToken).then((decodedIdToken) => {
//     console.log('ID Token correctly decoded', decodedIdToken);
//     req.user = decodedIdToken;
//     return next();
//   }).catch((error) => {
//     console.error('Error while verifying Firebase ID token:', error);
//     res.status(403).send('Unauthorized');
//   });


//        next() :> obj

        // if
        //     (!(authorization |> isNull))|| 
        //     !req.headers?authorization?startsWith("Bearer ")) &&
        //     !(req.cookies && req.cookies.__session)
        // then
        //     res.status(403).send("Unauthorized")
        //     return (null :?> obj)

        // else
        //     return next() :> obj
        // let headers = req.headers
        // printfn "%O" (headers.GetType())
        // next() :> obj

    let validateFirebaseIdToken' = new System.Func<express.Request, express.Response, obj -> unit, obj>(validateFirebaseIdToken)

    let expressApp = express.Invoke()

//    expressApp
    expressApp.``use``(cors({origin = "*"})) |> ignore
    expressApp.``use``(cookieParser()) |> ignore
    expressApp.``use``(validateFirebaseIdToken) |> ignore
    expressApp.get
        (U2.Case1 "/hello", 
            fun (req:express.Request) (res:express.Response) _ ->
                res.send(sprintf "Hello %O" req.user) |> box)
    |> ignore

    let app = https.onRequest expressApp


//     const validateFirebaseIdToken = (req, res, next) => {
//   console.log('Check if request is authorized with Firebase ID token');

//   if ((!req.headers.authorization || !req.headers.authorization.startsWith('Bearer ')) &&
//       !(req.cookies && req.cookies.__session)) {
//     console.error('No Firebase ID token was passed as a Bearer token in the Authorization header.',
//         'Make sure you authorize your request by providing the following HTTP header:',
//         'Authorization: Bearer <Firebase ID Token>',
//         'or by passing a "__session" cookie.');
//     res.status(403).send('Unauthorized');
//     return;
//   }

//   let idToken;
//   if (req.headers.authorization && req.headers.authorization.startsWith('Bearer ')) {
//     console.log('Found "Authorization" header');
//     // Read the ID Token from the Authorization header.
//     idToken = req.headers.authorization.split('Bearer ')[1];
//   } else if(req.cookies) {
//     console.log('Found "__session" cookie');
//     // Read the ID Token from cookie.
//     idToken = req.cookies.__session;
//   } else {
//     // No cookie
//     res.status(403).send('Unauthorized');
//     return;
//   }
//   admin.auth().verifyIdToken(idToken).then((decodedIdToken) => {
//     console.log('ID Token correctly decoded', decodedIdToken);
//     req.user = decodedIdToken;
//     return next();
//   }).catch((error) => {
//     console.error('Error while verifying Firebase ID token:', error);
//     res.status(403).send('Unauthorized');
//   });
// };