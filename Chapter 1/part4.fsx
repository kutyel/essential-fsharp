type CustomerType =
    | Registered of IsEligible: bool
    | Guest

type Customer = { Id: string; Type: CustomerType }

let calculateTotal customer spend : decimal =
    let discount =
        match customer.Type with
        | Registered(IsEligible = true) when spend >= 100.0M -> spend * 0.1M
        | _ -> 0.0M

    spend - discount

let john = { Type = Registered true; Id = "John" }
let mary = { Type = Registered true; Id = "Mary" }

let richard =
    { Type = Registered false
      Id = "Richard" }

let sarah = { Type = Guest; Id = "Sarah" }

let assertJohn = 90.0M = calculateTotal john 100.0M
let assertMary = 99.0M = calculateTotal mary 99.0M
let assertRichard = 100.0M = calculateTotal richard 100.0M
let assertSarah = 100.0M = calculateTotal sarah 100.0M
