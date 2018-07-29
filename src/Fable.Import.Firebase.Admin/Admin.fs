namespace Fable.Import.Firebase
open Fable.Core

[<AutoOpen>]
module Admin =
    type [<AllowNullLiteral>] Identities =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set

    type [<AllowNullLiteral>] Firebase =
        abstract identities: Identities with get, set
        abstract sign_in_provider: string with get, set
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set

    type [<AllowNullLiteral>] DecodedIdToken =
        abstract aud: string with get, set
        abstract auth_time: float with get, set
        abstract exp: float with get, set
        abstract firebase: Firebase with get, set
        abstract iat: float with get, set
        abstract iss: string with get, set
        abstract sub: string with get, set
        abstract uid: string with get, set
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set
