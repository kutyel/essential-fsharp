let items = [ 1..5 ]

let readList items =
    match items with
    | [] -> "Empty list"
    | [ head ] -> $"Head: {head}"
    | head :: tail -> sprintf "Head: %A and Tail: %A" head tail

let emptyList = readList []
let multipleList = readList items
let singleItemList = readList [ 1 ]
