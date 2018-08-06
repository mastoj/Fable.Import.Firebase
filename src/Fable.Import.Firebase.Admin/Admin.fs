module rec Fable.Import.Firebase
open Fable.Core
open Fable.Import.JS
open System

let [<Import("*","firebase-admin")>] admin: Admin.IExports = jsNative

module Admin =
    let [<Import("credential","firebase-admin/admin")>] credential: Credential.IExports = jsNative
    let [<Import("database","firebase-admin/admin")>] database: Database.IExports = jsNative

    type [<AllowNullLiteral>] IExports =
            abstract SDK_VERSION: string
            abstract apps: ResizeArray<Admin.App.App option>
            abstract app: ?name: string -> Admin.App.App
            abstract auth: ?app: Admin.App.App -> Admin.Auth.Auth
            abstract database: ?app: Admin.App.App -> Admin.Database.Database
            abstract messaging: ?app: Admin.App.App -> Admin.Messaging.Messaging
            abstract storage: ?app: Admin.App.App -> Admin.Storage.Storage
            abstract firestore: ?app: Admin.App.App -> Admin.Firestore.Firestore
            abstract instanceId: ?app: Admin.App.App -> Admin.InstanceId.InstanceId
            abstract initializeApp: ?options: Admin.AppOptions * ?name: string -> Admin.App.App

    type [<AllowNullLiteral>] AppOptions =
        abstract credential: Admin.Credential.Credential option with get, set
        abstract databaseAuthVariableOverride: Object option with get, set
        abstract databaseURL: string option with get, set
        abstract storageBucket: string option with get, set
        abstract projectId: string option with get, set

    type [<AllowNullLiteral>] ServiceAccount =
        abstract projectId: string option with get, set
        abstract clientEmail: string option with get, set
        abstract privateKey: string option with get, set

    type [<AllowNullLiteral>] FirebaseError =
        abstract code: string with get, set
        abstract message: string with get, set
        abstract stack: string with get, set
        abstract toJSON: unit -> Object

    type [<AllowNullLiteral>] FirebaseArrayIndexError =
        abstract index: float with get, set
        abstract error: FirebaseError with get, set

    type [<AllowNullLiteral>] GoogleOAuthAccessToken =
        abstract access_token: string with get, set
        abstract expires_in: float with get, set

    module Credential =

        type [<AllowNullLiteral>] IExports =
            abstract applicationDefault: unit -> Admin.Credential.Credential
            abstract cert: serviceAccountPathOrObject: U2<string, Admin.ServiceAccount> -> Admin.Credential.Credential
            abstract refreshToken: refreshTokenPathOrObject: U2<string, Object> -> Admin.Credential.Credential

        type [<AllowNullLiteral>] Credential =
            abstract getAccessToken: unit -> Promise<Admin.GoogleOAuthAccessToken>


    module App =

        type [<AllowNullLiteral>] App =
            abstract name: string with get, set
            abstract options: Admin.AppOptions with get, set
            abstract auth: unit -> Admin.Auth.Auth
            abstract database: ?url: string -> Admin.Database.Database
            abstract firestore: unit -> Admin.Firestore.Firestore
            abstract instanceId: unit -> Admin.InstanceId.InstanceId
            abstract messaging: unit -> Admin.Messaging.Messaging
            abstract storage: unit -> Admin.Storage.Storage
            abstract delete: unit -> Promise<unit>

    module Database =

        type [<AllowNullLiteral>] IExports =
            abstract enableLogging: ?logger: U2<bool, (string -> obj option)> * ?persistent: bool -> obj option

        type [<AllowNullLiteral>] Database =
            abstract app: Admin.App.App with get, set
            abstract goOffline: unit -> unit
            abstract goOnline: unit -> unit
            abstract ref: ?path: U2<string, Admin.Database.Reference> -> Admin.Database.Reference
            abstract refFromURL: url: string -> Admin.Database.Reference

        type [<AllowNullLiteral>] DataSnapshot =
            abstract key: string option with get, set
            abstract ref: Admin.Database.Reference with get, set
            abstract child: path: string -> Admin.Database.DataSnapshot
            abstract exists: unit -> bool
            abstract exportVal: unit -> obj option
            abstract forEach: action: (Admin.Database.DataSnapshot -> bool) -> bool
            abstract getPriority: unit -> U2<string, float> option
            abstract hasChild: path: string -> bool
            abstract hasChildren: unit -> bool
            abstract numChildren: unit -> float
            abstract toJSON: unit -> Object option
            abstract ``val``: unit -> obj option

        type [<AllowNullLiteral>] OnDisconnect =
            abstract cancel: ?onComplete: (Error option -> obj option) -> Promise<unit>
            abstract remove: ?onComplete: (Error option -> obj option) -> Promise<unit>
            abstract set: value: obj option * ?onComplete: (Error option -> obj option) -> Promise<unit>
            abstract setWithPriority: value: obj option * priority: U2<float, string> option * ?onComplete: (Error option -> obj option) -> Promise<unit>
            abstract update: values: Object * ?onComplete: (Error option -> obj option) -> Promise<unit>

        type [<StringEnum>] [<RequireQualifiedAccess>] EventType =
            | Value
            | Child_added
            | Child_changed
            | Child_moved
            | Child_removed
            
        type [<AllowNullLiteral>] Query =
            abstract ref: Admin.Database.Reference with get, set
            abstract endAt: value: U3<float, string, bool> option * ?key: string -> Admin.Database.Query
            abstract equalTo: value: U3<float, string, bool> option * ?key: string -> Admin.Database.Query
            abstract isEqual: other: Admin.Database.Query option -> bool
            abstract limitToFirst: limit: float -> Admin.Database.Query
            abstract limitToLast: limit: float -> Admin.Database.Query
            abstract off: ?eventType: Admin.Database.EventType * ?callback: (Admin.Database.DataSnapshot -> string option -> obj option) * ?context: Object option -> unit
            abstract on: eventType: Admin.Database.EventType * callback: (Admin.Database.DataSnapshot option -> string -> obj option) * ?cancelCallbackOrContext: Object option * ?context: Object option -> (Admin.Database.DataSnapshot option -> string -> obj option)
            abstract once: eventType: Admin.Database.EventType * ?successCallback: (Admin.Database.DataSnapshot -> string -> obj option) * ?failureCallbackOrContext: Object option * ?context: Object option -> Promise<obj option>
            abstract orderByChild: path: string -> Admin.Database.Query
            abstract orderByKey: unit -> Admin.Database.Query
            abstract orderByPriority: unit -> Admin.Database.Query
            abstract orderByValue: unit -> Admin.Database.Query
            abstract startAt: value: U3<float, string, bool> option * ?key: string -> Admin.Database.Query
            abstract toJSON: unit -> Object
            abstract toString: unit -> string

        type [<AllowNullLiteral>] Reference =
            inherit Admin.Database.Query
            abstract key: string option with get, set
            abstract parent: Admin.Database.Reference option with get, set
            abstract root: Admin.Database.Reference with get, set
            abstract path: string with get, set
            abstract child: path: string -> Admin.Database.Reference
            abstract onDisconnect: unit -> Admin.Database.OnDisconnect
            abstract push: ?value: obj option * ?onComplete: (Error option -> obj option) -> Admin.Database.ThenableReference
            abstract remove: ?onComplete: (Error option -> obj option) -> Promise<unit>
            abstract set: value: obj option * ?onComplete: (Error option -> obj option) -> Promise<unit>
            abstract setPriority: priority: U2<string, float> option * onComplete: (Error option -> obj option) -> Promise<unit>
            abstract setWithPriority: newVal: obj option * newPriority: U2<string, float> option * ?onComplete: (Error option -> obj option) -> Promise<unit>
            abstract transaction: transactionUpdate: (obj option -> obj option) * ?onComplete: (Error option -> bool -> Admin.Database.DataSnapshot option -> obj option) * ?applyLocally: bool -> Promise<obj>
            abstract update: values: Object * ?onComplete: (Error option -> obj option) -> Promise<unit>

        type [<AllowNullLiteral>] ThenableReference =
            inherit Admin.Database.Reference
            inherit PromiseLike<obj option>

    module Auth =
        type [<AllowNullLiteral>] UserMetadata =
            abstract lastSignInTime: string with get, set
            abstract creationTime: string with get, set
            abstract toJSON: unit -> Object

        type [<AllowNullLiteral>] UserInfo =
            abstract uid: string with get, set
            abstract displayName: string with get, set
            abstract email: string with get, set
            abstract phoneNumber: string with get, set
            abstract photoURL: string with get, set
            abstract providerId: string with get, set
            abstract toJSON: unit -> Object

        type [<AllowNullLiteral>] UserRecord =
            abstract uid: string with get, set
            abstract email: string with get, set
            abstract emailVerified: bool with get, set
            abstract displayName: string with get, set
            abstract phoneNumber: string with get, set
            abstract photoURL: string with get, set
            abstract disabled: bool with get, set
            abstract metadata: Admin.Auth.UserMetadata with get, set
            abstract providerData: ResizeArray<Admin.Auth.UserInfo> with get, set
            abstract passwordHash: string option with get, set
            abstract passwordSalt: string option with get, set
            abstract customClaims: Object option with get, set
            abstract tokensValidAfterTime: string option with get, set
            abstract toJSON: unit -> Object

        type [<AllowNullLiteral>] UpdateRequest =
            abstract displayName: string option with get, set
            abstract email: string option with get, set
            abstract emailVerified: bool option with get, set
            abstract phoneNumber: string option with get, set
            abstract photoURL: string option with get, set
            abstract disabled: bool option with get, set
            abstract password: string option with get, set

        type [<AllowNullLiteral>] CreateRequest =
            inherit UpdateRequest
            abstract uid: string option with get, set

        type [<AllowNullLiteral>] ListUsersResult =
            abstract users: ResizeArray<Admin.Auth.UserRecord> with get, set
            abstract pageToken: string option with get, set

        type [<StringEnum>] [<RequireQualifiedAccess>] HashAlgorithmType =
            | [<CompiledName "SCRYPT">] SCRYPT
            | [<CompiledName "STANDARD_SCRYPT">] STANDARD_SCRYPT
            | [<CompiledName "HMAC_SHA512">] HMAC_SHA512
            | [<CompiledName "HMAC_SHA256">] HMAC_SHA256
            | [<CompiledName "HMAC_SHA1">] HMAC_SHA1
            | [<CompiledName "HMAC_MD5">] HMAC_MD5
            | [<CompiledName "MD5">] MD5
            | [<CompiledName "PBKDF_SHA1">] PBKDF_SHA1
            | [<CompiledName "BCRYPT">] BCRYPT
            | [<CompiledName "PBKDF2_SHA256">] PBKDF2_SHA256
            | [<CompiledName "SHA512">] SHA512
            | [<CompiledName "SHA256">] SHA256
            | [<CompiledName "SHA1">] SHA1

        type [<AllowNullLiteral>] UserImportOptions =
            abstract hash: obj with get, set

        type [<AllowNullLiteral>] UserImportResult =
            abstract failureCount: float with get, set
            abstract successCount: float with get, set
            abstract errors: ResizeArray<Admin.FirebaseArrayIndexError> with get, set

        type [<AllowNullLiteral>] UserImportRecord =
            abstract uid: string with get, set
            abstract email: string option with get, set
            abstract emailVerified: bool option with get, set
            abstract displayName: string option with get, set
            abstract phoneNumber: string option with get, set
            abstract photoURL: string option with get, set
            abstract disabled: bool option with get, set
            abstract metadata: obj option with get, set
            abstract providerData: ResizeArray<obj> option with get, set
            abstract customClaims: Object option with get, set
            abstract passwordHash: Buffer option with get, set
            abstract passwordSalt: Buffer option with get, set

        type [<AllowNullLiteral>] SessionCookieOptions =
            abstract expiresIn: float with get, set

        type [<AllowNullLiteral>] Auth =
            abstract app: Admin.App.App with get, set
            abstract createCustomToken: uid: string * ?developerClaims: Object -> Promise<string>
            abstract createUser: properties: Admin.Auth.CreateRequest -> Promise<Admin.Auth.UserRecord>
            abstract deleteUser: uid: string -> Promise<unit>
            abstract getUser: uid: string -> Promise<Admin.Auth.UserRecord>
            abstract getUserByEmail: email: string -> Promise<Admin.Auth.UserRecord>
            abstract getUserByPhoneNumber: phoneNumber: string -> Promise<Admin.Auth.UserRecord>
            abstract listUsers: ?maxResults: float * ?pageToken: string -> Promise<Admin.Auth.ListUsersResult>
            abstract updateUser: uid: string * properties: Admin.Auth.UpdateRequest -> Promise<Admin.Auth.UserRecord>
            abstract verifyIdToken: idToken: string * ?checkRevoked: bool -> Promise<Admin.Auth.DecodedIdToken>
            abstract setCustomUserClaims: uid: string * customUserClaims: Object -> Promise<unit>
            abstract revokeRefreshTokens: uid: string -> Promise<unit>
            abstract importUsers: users: ResizeArray<Admin.Auth.UserImportRecord> * ?options: Admin.Auth.UserImportOptions -> Promise<Admin.Auth.UserImportResult>
            abstract createSessionCookie: idToken: string * sessionCookieOptions: Admin.Auth.SessionCookieOptions -> Promise<string>
            abstract verifySessionCookie: sessionCookie: string * ?checkForRevocation: bool -> Promise<Admin.Auth.DecodedIdToken>

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

    module Messaging =

        type Message =
            U3<TokenMessage, TopicMessage, ConditionMessage>

        [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
        module Message =
            let ofTokenMessage v: Message = v |> U3.Case1
            let isTokenMessage (v: Message) = match v with U3.Case1 _ -> true | _ -> false
            let asTokenMessage (v: Message) = match v with U3.Case1 o -> Some o | _ -> None
            let ofTopicMessage v: Message = v |> U3.Case2
            let isTopicMessage (v: Message) = match v with U3.Case2 _ -> true | _ -> false
            let asTopicMessage (v: Message) = match v with U3.Case2 o -> Some o | _ -> None
            let ofConditionMessage v: Message = v |> U3.Case3
            let isConditionMessage (v: Message) = match v with U3.Case3 _ -> true | _ -> false
            let asConditionMessage (v: Message) = match v with U3.Case3 o -> Some o | _ -> None

        type [<AllowNullLiteral>] AndroidConfig =
            abstract collapseKey: string option with get, set
            abstract priority: U2<string, string> option with get, set
            abstract ttl: float option with get, set
            abstract restrictedPackageName: string option with get, set
            abstract data: obj option with get, set
            abstract notification: AndroidNotification option with get, set

        type [<AllowNullLiteral>] AndroidNotification =
            abstract title: string option with get, set
            abstract body: string option with get, set
            abstract icon: string option with get, set
            abstract color: string option with get, set
            abstract sound: string option with get, set
            abstract tag: string option with get, set
            abstract clickAction: string option with get, set
            abstract bodyLocKey: string option with get, set
            abstract bodyLocArgs: ResizeArray<string> option with get, set
            abstract titleLocKey: string option with get, set
            abstract titleLocArgs: ResizeArray<string> option with get, set

        type [<AllowNullLiteral>] ApnsConfig =
            abstract headers: obj option with get, set
            abstract payload: ApnsPayload option with get, set

        type [<AllowNullLiteral>] ApnsPayload =
            abstract aps: Aps with get, set
            [<Emit "$0[$1]{{=$2}}">] abstract Item: customData: string -> obj with get, set

        type [<AllowNullLiteral>] Aps =
            abstract alert: U2<string, ApsAlert> option with get, set
            abstract badge: float option with get, set
            abstract sound: string option with get, set
            abstract contentAvailable: bool option with get, set
            abstract mutableContent: bool option with get, set
            abstract category: string option with get, set
            abstract threadId: string option with get, set
            [<Emit "$0[$1]{{=$2}}">] abstract Item: customData: string -> obj option with get, set

        type [<AllowNullLiteral>] ApsAlert =
            abstract title: string option with get, set
            abstract body: string option with get, set
            abstract locKey: string option with get, set
            abstract locArgs: ResizeArray<string> option with get, set
            abstract titleLocKey: string option with get, set
            abstract titleLocArgs: ResizeArray<string> option with get, set
            abstract actionLocKey: string option with get, set
            abstract launchImage: string option with get, set

        type [<AllowNullLiteral>] Notification =
            abstract title: string option with get, set
            abstract body: string option with get, set

        type [<AllowNullLiteral>] WebpushConfig =
            abstract headers: obj option with get, set
            abstract data: obj option with get, set
            abstract notification: WebpushNotification option with get, set

        type [<AllowNullLiteral>] WebpushNotification =
            abstract title: string option with get, set
            abstract body: string option with get, set
            abstract icon: string option with get, set

        type [<AllowNullLiteral>] DataMessagePayload =
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string with get, set

        type [<AllowNullLiteral>] NotificationMessagePayload =
            abstract tag: string option with get, set
            abstract body: string option with get, set
            abstract icon: string option with get, set
            abstract badge: string option with get, set
            abstract color: string option with get, set
            abstract sound: string option with get, set
            abstract title: string option with get, set
            abstract bodyLocKey: string option with get, set
            abstract bodyLocArgs: string option with get, set
            abstract clickAction: string option with get, set
            abstract titleLocKey: string option with get, set
            abstract titleLocArgs: string option with get, set
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string option with get, set

        type [<AllowNullLiteral>] MessagingPayload =
            abstract data: Admin.Messaging.DataMessagePayload option with get, set
            abstract notification: Admin.Messaging.NotificationMessagePayload option with get, set

        type [<AllowNullLiteral>] MessagingOptions =
            abstract dryRun: bool option with get, set
            abstract priority: string option with get, set
            abstract timeToLive: float option with get, set
            abstract collapseKey: string option with get, set
            abstract mutableContent: bool option with get, set
            abstract contentAvailable: bool option with get, set
            abstract restrictedPackageName: string option with get, set
            [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option option with get, set

        type [<AllowNullLiteral>] MessagingDeviceResult =
            abstract error: Admin.FirebaseError option with get, set
            abstract messageId: string option with get, set
            abstract canonicalRegistrationToken: string option with get, set

        type [<AllowNullLiteral>] MessagingDevicesResponse =
            abstract canonicalRegistrationTokenCount: float with get, set
            abstract failureCount: float with get, set
            abstract multicastId: float with get, set
            abstract results: ResizeArray<Admin.Messaging.MessagingDeviceResult> with get, set
            abstract successCount: float with get, set

        type [<AllowNullLiteral>] MessagingDeviceGroupResponse =
            abstract successCount: float with get, set
            abstract failureCount: float with get, set
            abstract failedRegistrationTokens: ResizeArray<string> with get, set

        type [<AllowNullLiteral>] MessagingTopicResponse =
            abstract messageId: float with get, set

        type [<AllowNullLiteral>] MessagingConditionResponse =
            abstract messageId: float with get, set

        type [<AllowNullLiteral>] MessagingTopicManagementResponse =
            abstract failureCount: float with get, set
            abstract successCount: float with get, set
            abstract errors: ResizeArray<Admin.FirebaseArrayIndexError> with get, set

        type [<AllowNullLiteral>] Messaging =
            abstract app: Admin.App.App with get, set
            abstract send: message: Admin.Messaging.Message * ?dryRun: bool -> Promise<string>
            abstract sendToDevice: registrationToken: U2<string, ResizeArray<string>> * payload: Admin.Messaging.MessagingPayload * ?options: Admin.Messaging.MessagingOptions -> Promise<Admin.Messaging.MessagingDevicesResponse>
            abstract sendToDeviceGroup: notificationKey: string * payload: Admin.Messaging.MessagingPayload * ?options: Admin.Messaging.MessagingOptions -> Promise<Admin.Messaging.MessagingDeviceGroupResponse>
            abstract sendToTopic: topic: string * payload: Admin.Messaging.MessagingPayload * ?options: Admin.Messaging.MessagingOptions -> Promise<Admin.Messaging.MessagingTopicResponse>
            abstract sendToCondition: condition: string * payload: Admin.Messaging.MessagingPayload * ?options: Admin.Messaging.MessagingOptions -> Promise<Admin.Messaging.MessagingConditionResponse>
            abstract subscribeToTopic: registrationToken: string * topic: string -> Promise<Admin.Messaging.MessagingTopicManagementResponse>
            abstract subscribeToTopic: registrationTokens: ResizeArray<string> * topic: string -> Promise<Admin.Messaging.MessagingTopicManagementResponse>
            abstract unsubscribeFromTopic: registrationToken: string * topic: string -> Promise<Admin.Messaging.MessagingTopicManagementResponse>
            abstract unsubscribeFromTopic: registrationTokens: ResizeArray<string> * topic: string -> Promise<Admin.Messaging.MessagingTopicManagementResponse>

    module Storage =

        type [<AllowNullLiteral>] Storage =
            abstract app: Admin.App.App with get, set
            abstract bucket: ?name: string -> Bucket

    module InstanceId =

        type [<AllowNullLiteral>] InstanceId =
            abstract app: Admin.App.App with get, set
            abstract deleteInstanceId: instanceId: string -> Promise<unit>

    type [<AllowNullLiteral>] BaseMessage =
        abstract data: obj option with get, set
        abstract notification: Admin.Messaging.Notification option with get, set
        abstract android: Admin.Messaging.AndroidConfig option with get, set
        abstract webpush: Admin.Messaging.WebpushConfig option with get, set
        abstract apns: Admin.Messaging.ApnsConfig option with get, set

    type [<AllowNullLiteral>] TokenMessage =
        inherit BaseMessage
        abstract token: string with get, set

    type [<AllowNullLiteral>] TopicMessage =
        inherit BaseMessage
        abstract topic: string with get, set

    type [<AllowNullLiteral>] ConditionMessage =
        inherit BaseMessage
        abstract condition: string with get, set

    module Firestore =
        type [<AllowNullLiteral>] DocRef =
            abstract id: string with get, set

        type [<AllowNullLiteral>] Document =
            inherit DocRef
            abstract data: unit -> obj
            abstract delete: unit -> Promise<unit>
            abstract set<'a> : 'a -> Promise<unit>
            abstract update<'a> : 'a -> Promise<unit>

        type [<AllowNullLiteral>] Collection =
            abstract add<'a> : ('a) -> Promise<DocRef>
            abstract get: unit -> Promise<Document[]>

        type [<AllowNullLiteral>] Firestore =
            abstract collection: string -> Collection
            abstract doc: string -> Document

    type [<AllowNullLiteral>] Bucket = interface end