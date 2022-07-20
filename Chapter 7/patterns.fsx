open System

let (|ValidDate|_|) (input: string) =
    match DateTime.TryParse(input) with
    | true, value -> Some value
    | false, _ -> None

let parse input =
    match input with
    | ValidDate dt -> printfn "%A" dt
    | _ -> printfn $"'{input}' is not a valid date"

parse "20-07-2022"
parse "Hello"

let (|IsDivisibleBy|_|) divisor n =
    if n % divisor = 0 then Some() else None

let calculate i =
    match i with
    | IsDivisibleBy 3 & IsDivisibleBy 5 -> "FizzBuzz"
    | IsDivisibleBy 3 -> "Fizz"
    | IsDivisibleBy 5 -> "Buzz"
    | _ -> i |> string

[ 1..15 ] |> List.map calculate

let (|NotDivisibleBy|_|) div n = if n % div <> 0 then Some() else None

let isLeapYear year =
    match year with
    | IsDivisibleBy 400 -> true
    | IsDivisibleBy 4 & NotDivisibleBy 100 -> true
    | _ -> false

[ 2000; 2001; 2020; 2023; 2024 ]
|> List.map isLeapYear

type Rank =
    | Ace
    | Two
    | Three
    | Four
    | Five
    | Six
    | Seven
    | Eight
    | Nine
    | Ten
    | Jack
    | Queen
    | King

type Suit =
    | Hearts
    | Clubs
    | Diamonds
    | Spades

type Card = Rank * Suit

let (|Red|Black|) (card: Card) =
    match card with
    | (_, Diamonds)
    | (_, Hearts) -> Red
    | (_, Clubs)
    | (_, Spades) -> Black

let describeColor card =
    match card with
    | Red -> "red"
    | Black -> "black"
    |> printf "The card is %s"

describeColor (Two, Hearts)

let (|CharacterCount|) (input: string) = input.Length

let (|ContainsANumber|) (input: string) =
    input |> Seq.filter Char.IsDigit |> Seq.length > 0

let (|IsValidPassword|) input =
    match input with
    | CharacterCount len when len < 8 -> (false, "Password must be at least 8 characters.")
    | ContainsANumber false -> (false, "Password must contain at least 1 digit.")
    | _ -> (true, "")

let setPassword input =
    match input with
    | IsValidPassword (true, _) as pwd -> Ok pwd
    | IsValidPassword (false, reason) -> Error $"Password not set: {reason}"

let badPassword = setPassword "password"
let goodPassword = setPassword "passw0rd"
