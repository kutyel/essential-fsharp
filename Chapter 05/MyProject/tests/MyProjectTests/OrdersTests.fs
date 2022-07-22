namespace OrderTests

open MyProject.Orders
open MyProject.Orders.Domain
open Xunit
open FsUnit

module ``Add item to order`` =

    [<Fact>]
    let ``when product does not exist in empty order`` () =
        let emptyOrder = { Id = 1; Items = [] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 3 } ] }

        let actual =
            emptyOrder
            |> addItem { ProductId = 1; Quanity = 3 }

        actual |> should equal expected

    [<Fact>]
    let ``when product does not exist in non-empty order`` () =
        let order =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 1 } ] }

        let expected =
            { Id = 1
              Items =
                [ { ProductId = 1; Quanity = 1 }
                  { ProductId = 2; Quanity = 3 } ] }

        let actual = order |> addItem { ProductId = 2; Quanity = 3 }

        actual |> should equal expected

    [<Fact>]
    let ``when product exists in non-empty order`` () =
        let order =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 1 } ] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 4 } ] }

        let actual = order |> addItem { ProductId = 1; Quanity = 3 }

        actual |> should equal expected

module ``Add multiple items to an order`` =

    [<Fact>]
    let ``when new products added to empty order`` () =
        let emptyOrder = { Id = 1; Items = [] }

        let expected =
            { Id = 1
              Items =
                [ { ProductId = 1; Quanity = 3 }
                  { ProductId = 2; Quanity = 5 } ] }

        let actual =
            emptyOrder
            |> addItems [ { ProductId = 1; Quanity = 3 }
                          { ProductId = 2; Quanity = 5 } ]

        actual |> should equal expected

    [<Fact>]
    let ``when new products and updated existing to order`` () =
        let order =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 1 } ] }

        let expected =
            { Id = 1
              Items =
                [ { ProductId = 1; Quanity = 2 }
                  { ProductId = 2; Quanity = 5 } ] }

        let actual =
            order
            |> addItems [ { ProductId = 1; Quanity = 1 }
                          { ProductId = 2; Quanity = 5 } ]

        actual |> should equal expected

module ``Removing a product`` =

    [<Fact>]
    let ``when remove all items of existing productId`` () =
        let order =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 1 } ] }

        let expected = { Id = 1; Items = [] }
        let actual = order |> removeProduct 1
        actual |> should equal expected

    [<Fact>]
    let ``should do nothing for non-existent productId`` () =
        let order =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 1 } ] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 1 } ] }

        let actual = order |> removeProduct 2
        actual |> should equal expected

module ``Reduce item quantity`` =

    [<Fact>]
    let ``reduce existing item quantity`` () =
        let order =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 5 } ] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 2 } ] }

        let actual = order |> reduceItem 1 3
        actual |> should equal expected

    [<Fact>]
    let ``reduce existing item and remove`` () =
        let order =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 1 } ] }

        let expected = { Id = 1; Items = [] }

        let actual = order |> reduceItem 1 1
        actual |> should equal expected

    [<Fact>]
    let ``reduce item with no quantity`` () =
        let order =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 5 } ] }

        let expected =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 5 } ] }

        let actual = order |> reduceItem 2 3
        actual |> should equal expected

    [<Fact>]
    let ``reduce item with no quantity for empty order`` () =
        let emptyOrder = { Id = 1; Items = [] }

        let expected = { Id = 1; Items = [] }

        let actual = emptyOrder |> reduceItem 2 3
        actual |> should equal expected

module ``Empty and order of all items`` =

    [<Fact>]
    let ``order with existing item`` () =
        let order =
            { Id = 1
              Items = [ { ProductId = 1; Quanity = 5 } ] }

        let expected = { Id = 1; Items = [] }

        let actual = order |> clearItems
        actual |> should equal expected

    [<Fact>]
    let ``order with existing items`` () =
        let order =
            { Id = 1
              Items =
                [ { ProductId = 1; Quanity = 5 }
                  { ProductId = 2; Quanity = 15 } ] }

        let expected = { Id = 1; Items = [] }

        let actual = order |> clearItems
        actual |> should equal expected

    [<Fact>]
    let ``empty order is unchanged`` () =
        let emptyOrder = { Id = 1; Items = [] }

        let expected = { Id = 1; Items = [] }

        let actual = emptyOrder |> clearItems
        actual |> should equal expected
