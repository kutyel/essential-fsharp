module Main exposing (Customer(..), calculateTotal)


type Customer
    = Eligible { id : String }
    | Registered { id : String }
    | Guest { id : String }


calculateTotal : Customer -> Double -> Double
calculateTotal customer spend =
    let
        discount =
            case customer of
                Eligible _ ->
                    if spend >= 100.0 then
                        spend * 0.1

                    else
                        0.0

                _ ->
                    0.0
    in
    spend - discount


john =
    Eligible { id = "John" }


assertJohn =
    calculateTotal john 100.0 == 90.0
