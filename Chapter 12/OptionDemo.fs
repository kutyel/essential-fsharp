namespace ComputationExpression

[<AutoOpen>]
module Option =

    type OptionBuilder() =
        member _.Bind(x, f) = Option.bind f x
        member _.Return(x) = Some x
        member _.ReturnFrom(x) = x

    let option = OptionBuilder()

module OptionDemo =

    let multiply x y = x * y
    let divide x y = if y = 0 then None else Some(x / y)

    let calculate x y =
        option {
            let! v = divide x y
            let t = multiply v x
            return! divide t y
        }
