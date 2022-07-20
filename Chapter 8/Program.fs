open System
open System.IO
open System.Text.RegularExpressions
open FsToolkit.ErrorHandling.ValidationCE

type Customer =
    { CustomerId: string
      Email: string
      IsEligible: string
      IsRegistered: string
      DateRegistered: string
      Discount: string }

type ValidatedCustomer =
    { CustomerId: string
      Email: string option
      IsEligible: bool
      IsRegistered: bool
      DateRegistered: DateTime option
      Discount: decimal option }

type DataReader = string -> Result<string seq, exn>

type ValidationError =
    | MissingData of name: string
    | InvalidData of name: string * value: string

// Sadly Records in F# are not Constructor Functions!! 😭
let create customerId email isEligible isRegistered dateRegistered discount =
    { CustomerId = customerId
      Email = email
      IsEligible = isEligible
      IsRegistered = isRegistered
      DateRegistered = dateRegistered
      Discount = discount }

let readFile: DataReader =
    fun path ->
        try
            File.ReadLines(path) |> Ok
        with
        | ex -> Error ex

let parseLine (line: string) : Customer option =
    match line.Split('|') with
    | [| customerId; email; eligible; registered; dateRegistered; discount |] ->
        Some
            { CustomerId = customerId
              Email = email
              IsEligible = eligible
              IsRegistered = registered
              DateRegistered = dateRegistered
              Discount = discount }
    | _ -> None


let output = Seq.iter (printfn "%A")

// Validation Active Patterns

let (|ParseRegex|_|) regex str =
    let m = Regex(regex).Match(str)

    if m.Success then
        Some(List.tail [ for x in m.Groups -> x.Value ])
    else
        None

let (|IsValidEmail|_|) input =
    match input with
    | ParseRegex ".*?@(.*)" [ _ ] -> Some input
    | _ -> None

let (|IsEmptyString|_|) (input: string) =
    if input.Trim() = "" then
        Some()
    else
        None

let (|IsDecimal|_|) (input: string) =
    let success, value = Decimal.TryParse input
    if success then Some value else None

let (|IsBoolean|_|) (input: string) =
    match input with
    | "1" -> Some true
    | "0" -> Some false
    | _ -> None

let (|IsValidDate|_|) (input: string) =
    let success, value = DateTime.TryParse input
    if success then Some value else None

// Validation functions

let validateCustomerId customerId =
    if customerId <> "" then
        Ok customerId
    else
        Error(MissingData "CustomerId")

let validateEmail email =
    if email <> "" then
        match email with
        | IsValidEmail _ -> Ok(Some email)
        | _ -> Error <| InvalidData("Email", email)
    else
        Ok None

let validateIsEligible isEligible =
    match isEligible with
    | IsBoolean b -> Ok b
    | _ -> Error <| InvalidData("IsEligible", isEligible)

let validateIsRegistered isRegistered =
    match isRegistered with
    | IsBoolean b -> Ok b
    | _ -> Error <| InvalidData("IsRegistered", isRegistered)

let validateDateRegistered dateRegistered =
    match dateRegistered with
    | IsEmptyString -> Ok None
    | IsValidDate dt -> Ok <| Some dt
    | _ ->
        Error
        <| InvalidData("DateRegistered", dateRegistered)

let validateDiscount discount =
    match discount with
    | IsEmptyString -> Ok None
    | IsDecimal value -> Ok(Some value)
    | _ -> Error <| InvalidData("Discount", discount)

let validate (input: Customer) : Result<ValidatedCustomer, ValidationError list> =
    // This is just an attempt at Haskell's DO notation 😎
    validation {
        let! customerId =
            input.CustomerId
            |> validateCustomerId
            |> Result.mapError List.singleton

        and! email =
            input.Email
            |> validateEmail
            |> Result.mapError List.singleton

        and! isEligible =
            input.IsEligible
            |> validateIsEligible
            |> Result.mapError List.singleton

        and! isRegistered =
            input.IsRegistered
            |> validateIsRegistered
            |> Result.mapError List.singleton

        and! dateRegistered =
            input.DateRegistered
            |> validateDateRegistered
            |> Result.mapError List.singleton

        and! discount =
            input.Discount
            |> validateDiscount
            |> Result.mapError List.singleton
        // And of course, we need return (=pure) to exit the Monadic/Applicative block 😉
        return create customerId email isEligible isRegistered dateRegistered discount
    }

let parse =
    Seq.skip 1
    >> Seq.map parseLine
    >> Seq.choose id // == filterMap
    >> Seq.map validate

let import (dataReader: DataReader) path =
    match path |> dataReader with
    | Ok data -> data |> parse |> output
    | Error ex -> printfn "Error: %A" ex.Message

[<EntryPoint>]
let main argv =
    Path.Combine(__SOURCE_DIRECTORY__, "resources", "customers.csv")
    |> import readFile

    0
