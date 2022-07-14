type Customer =
    | Registered of Id: string * IsEligible: bool
    | Guest of Id: string

let (|IsEligible|_|) customer =
    match customer with
    | Registered(IsEligible = true) -> Some()
    | _ -> None

let calculateTotal customer spend : decimal =
    let discount =
        match customer with
        | IsEligible when spend >= 100.0M -> spend * 0.1M
        | _ -> 0.0M

    spend - discount

let john = Registered(Id = "John", IsEligible = true)
let mary = Registered(Id = "Mary", IsEligible = true)
let richard = Registered(Id = "Richard", IsEligible = false)
let sarah = Guest(Id = "Sarah")

let areEqual expected actual = actual = expected

let assertJohn = areEqual 90.0M (calculateTotal john 100.0M)
let assertMary = areEqual 99.0M (calculateTotal mary 99.0M)
let assertRichard = areEqual 100.0M (calculateTotal richard 100.0M)
let assertSarah = areEqual 100.0M (calculateTotal sarah 100.0M)
