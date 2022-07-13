type Customer =
    | Eligible of Id:string
    | Registered of Id:string
    | Guest of Id:string

let calculateTotal customer spend : decimal =
    let discount =
        match customer with
        | Eligible _ when spend >= 100.0M -> spend * 0.1M
        | _ -> 0.0M
    spend - discount

let john = Eligible "John"
let mary = Eligible "Mary"
let richard = Registered "Richard"
let sarah = Guest "Sarah"

let assertJohn = 90.0M = calculateTotal john 100.0M
let assertMary = 99.0M = calculateTotal mary 99.0M
let assertRichard = 100.0M = calculateTotal richard 100.0M
let assertSarah = 100.0M = calculateTotal sarah 100.0M
