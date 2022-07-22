namespace MyProjectTests

open Xunit
open MyProject.Customer

module ``When upgrading customer`` =
    let customerVIP = { Id = 1; IsVip = true; Credit = 0.0M }

    let customerSTD =
        { Id = 2
          IsVip = false
          Credit = 100.0M }

    [<Fact>]
    let ``should give VIP customer more credit`` () =
        let expected =
            { Id = 1
              IsVip = true
              Credit = 100.0M }

        let actual = upgradeCustomer customerVIP
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``should convert eligible STD customer to VIP`` () =
        let expected =
            { Id = 2
              IsVip = true
              Credit = 200.0M }

        let actual = upgradeCustomer customerSTD
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``should not upgrade ineligible STD customer to VIP`` () =
        let expected =
            { Id = 3
              IsVip = false
              Credit = 100.0M }

        let actual =
            upgradeCustomer
                { customerSTD with
                    Id = 3
                    Credit = 50.0M }

        Assert.Equal(expected, actual)
