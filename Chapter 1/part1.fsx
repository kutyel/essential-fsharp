type Customer = { 
    Id: string
    IsEligible: bool
    IsRegistered: bool
}

let calculateTotal customer spend : decimal =
    let discount =
        if customer.IsEligible && spend >= 100.0M then
            (spend * 0.1M)
        else
            0.0M
    spend - discount

let john = {Id="John"; IsEligible=true; IsRegistered=true}
let mary = {Id="Mary"; IsEligible=true; IsRegistered=true}
let richard = {Id="Richard"; IsEligible=false; IsRegistered=true}
let sarah = {Id="Sarah"; IsEligible=false; IsRegistered=false}

let areEqual expected actual = actual = expected

let assertJohn = areEqual 90.0M (calculateTotal john 100.0M)
let assertMary = areEqual 99.0M (calculateTotal mary 99.0M)
let assertRichard = areEqual 100.0M (calculateTotal richard 100.0M)
let assertSarah = areEqual 100.0M (calculateTotal sarah 100.0M)
