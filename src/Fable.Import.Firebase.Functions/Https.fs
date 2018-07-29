module Fable.Import.Firebase.Functions
open Fable.Import.Firebase.Admin
open Fable.Import
open Fable.Import.JS
open Fable.Core

[<AutoOpen>]
module rec Https =

    type [<AllowNullLiteral>] HttpsFunction =
        interface end

    type [<AllowNullLiteral>] IExports =
        abstract onRequest: handler: (express.Request -> express.Response -> unit) -> HttpsFunction
        abstract onRequest: handler: express.Application -> HttpsFunction
        abstract HttpsError: HttpsErrorStatic
        /// <summary>Declares a callable method for clients to call using a Firebase SDK.</summary>
        /// <param name="handler">A method that takes a data and context and returns a value.</param>
        abstract onCall: handler: (obj option -> CallableContext -> U2<obj option, Promise<obj option>>) -> HttpsFunction

    type [<StringEnum>] [<RequireQualifiedAccess>] FunctionsErrorCode =
        | Ok
        | Cancelled
        | Unknown
        | [<CompiledName "invalid-argument">] InvalidArgument
        | [<CompiledName "deadline-exceeded">] DeadlineExceeded
        | [<CompiledName "not-found">] NotFound
        | [<CompiledName "already-exists">] AlreadyExists
        | [<CompiledName "permission-denied">] PermissionDenied
        | [<CompiledName "resource-exhausted">] ResourceExhausted
        | [<CompiledName "failed-precondition">] FailedPrecondition
        | Aborted
        | [<CompiledName "out-of-range">] OutOfRange
        | Unimplemented
        | Internal
        | Unavailable
        | [<CompiledName "data-loss">] DataLoss
        | Unauthenticated

    /// An explicit error that can be thrown from a handler to send an error to the
    /// client that called the function.
    type [<AllowNullLiteral>] HttpsError =
        inherit Error
        /// A standard error code that will be returned to the client. This also
        /// determines the HTTP status code of the response, as defined in code.proto.
        abstract code: FunctionsErrorCode
        /// Extra data to be converted to JSON and included in the error response.
        abstract details: obj option

    /// An explicit error that can be thrown from a handler to send an error to the
    /// client that called the function.
    type [<AllowNullLiteral>] HttpsErrorStatic =
        [<Emit "new $0($1...)">] abstract Create: code: FunctionsErrorCode * ?message: string * ?details: obj option -> HttpsError

    type [<AllowNullLiteral>] Auth =
        abstract uid: string with get, set
        abstract token: DecodedIdToken

    /// The interface for metadata for the API as passed to the handler.
    type [<AllowNullLiteral>] CallableContext =
        /// The result of decoding and verifying a Firebase Auth ID token.
        abstract auth: Auth option with get, set
        /// An unverified token for a Firebase Instance ID.
        abstract instanceIdToken: string option with get, set
        /// The raw request handled by the callable.
        abstract rawRequest: Express.Request with get, set

let [<Fable.Core.Import("https", "firebase-functions")>] https: IExports = jsNative
