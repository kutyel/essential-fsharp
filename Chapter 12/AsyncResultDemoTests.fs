namespace ComputationExpression

module AsyncResultDemoTests =

    open AsyncResultDemo

    [<Literal>]
    let BadPassword = "notpassword"

    [<Literal>]
    let NotValidUser = "notvalid"

    let isOk (input: Result<_, _>) : bool =
        match input with
        | Ok _ -> true
        | _ -> false

    let matchError (error: LoginError) (input: Result<_, LoginError>) =
        match input with
        | Error ex -> ex = error
        | _ -> false

    let runWithValidPassword (username: string) =
        login username ValidPassword
        |> Async.RunSynchronously

    let success =
        let result = runWithValidPassword ValidUser
        result |> isOk

    let badPassword =
        let result =
            login ValidUser BadPassword
            |> Async.RunSynchronously

        result |> matchError InvalidPwd

    let invalidUser =
        runWithValidPassword NotValidUser
        |> matchError InvalidUser

    let isSuspended =
        runWithValidPassword SuspendedUser
        |> matchError (UserBannedOrSuspended |> Unauthorised)

    let isBanned =
        let result = runWithValidPassword BannedUser

        result
        |> matchError (UserBannedOrSuspended |> Unauthorised)

    let hasBadLuck =
        let result = runWithValidPassword BadLuckUser

        result
        |> matchError (AuthErrorMessage |> BadThingHappened |> TokenErr)
