type ValidationError = InputOutOfRange of string

// Newtype! 😎
type Spend = private Spend of decimal

module Spend =
    let value input = input |> fun (Spend value) -> value

    let create input =
        if input >= 0.0M && input <= 1000.0M then
            Ok(Spend input)
        else
            Error
            <| InputOutOfRange "You can only spend between 0 and 1000"

type CustomerId = CustomerId of string
type RegisteredCustomer = { Id: CustomerId }
type UnregisteredCustomer = { Id: CustomerId }

type Customer =
    | Eligible of RegisteredCustomer
    | Registered of RegisteredCustomer
    | Guest of UnregisteredCustomer

type Total = decimal
type DiscountPercentage = decimal

module Customer =
    let calculateDiscountPercentage spend customer : DiscountPercentage =
        match customer with
        | Eligible _ when Spend.value spend >= 100.0M -> 0.1M
        | _ -> 0.0M

    let calculateTotal customer spend : Total =
        customer
        |> calculateDiscountPercentage spend
        |> fun discountPercetage -> Spend.value spend * (1.0M - discountPercetage)

let john = Eligible { Id = CustomerId "John" }
let mary = Eligible { Id = CustomerId "Mary" }
let richard = Registered { Id = CustomerId "Richard" }
let sarah = Guest { Id = CustomerId "Sarah" }

let isEqualTo expected actual = actual = expected

let assertEqual customer spend expected =
    Spend.create spend
    |> Result.map (Customer.calculateTotal customer)
    |> isEqualTo (Ok expected)

let assertJohn = assertEqual john 100.0M 90.0M
let assertMary = assertEqual mary 99.0M 99.0M
let assertRichard = assertEqual richard 100.0M 100.0M
let assertSarah = assertEqual sarah 100.0M 100.0M
