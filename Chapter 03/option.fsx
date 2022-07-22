open System

let tryParseDateTime (input: string) =
    match DateTime.TryParse input with
    | true, result -> Some result
    | _ -> None

let isDate = tryParseDateTime "2019-01-22"
let isNotDate = tryParseDateTime "fooo"

type PersonName =
    { FirstName: string
      MiddleName: string option
      LastName: string }

let me =
    { FirstName = "Flavio"
      MiddleName = Some "Luis"
      LastName = "Corpa" }
