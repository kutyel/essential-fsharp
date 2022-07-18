namespace MyProject.Orders

type Item = { ProductId: int; Quanity: int }

type Order = { Id: int; Items: Item list }

module Domain =

    let recalculate =
        List.groupBy (fun i -> i.ProductId)
        >> List.map (fun (id, items) ->
            { ProductId = id
              Quanity = items |> List.sumBy (fun i -> i.Quanity) })
        >> List.sortBy (fun i -> i.ProductId)

    let addItem item order =
        { order with Items = item :: order.Items |> recalculate }

    let addItems newItems order =
        { order with Items = newItems @ order.Items |> recalculate }

    let removeProduct productId order =
        let items =
            order.Items
            |> List.filter (fun x -> x.ProductId <> productId)
            |> List.sortBy (fun i -> i.ProductId)

        { order with Items = items }

    let reduceItem productId quantity order =
        let items =
            { ProductId = productId
              Quanity = -quantity }
            :: order.Items
            |> recalculate
            |> List.filter (fun x -> x.Quanity > 0)

        { order with Items = items }

    let clearItems order = { order with Items = [] }
