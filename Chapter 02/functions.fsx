type Customer =
    { Id: int
      IsVip: bool
      Credit: decimal }

let getPurchases customer =
    let purchases =
        if customer.Id % 2 = 0 then
            120M
        else
            80M

    (customer, purchases)

let tryPromoteToVip purchases =
    let (customer, amount) = purchases

    if amount > 100M then
        { customer with IsVip = true }
    else
        customer

let increaseCreditIfVip customer =
    let increase = if customer.IsVip then 100M else 50M
    { customer with Credit = customer.Credit + increase }

// Function composition, my favourite! 🙌
let upgradeCustomer =
    getPurchases
    >> tryPromoteToVip
    >> increaseCreditIfVip

let customerVIP = { Id = 1; IsVip = true; Credit = 0.0M }

let customerSTD =
    { Id = 2
      IsVip = false
      Credit = 100.0M }

let assertVIP =
    upgradeCustomer customerVIP = { Id = 1
                                    IsVip = true
                                    Credit = 100.0M }

let assertSTDtoVIP =
    upgradeCustomer customerSTD = { Id = 2
                                    IsVip = true
                                    Credit = 200.0M }

let assertSTD =
    upgradeCustomer
        { customerSTD with
            Id = 3
            Credit = 50.0M } = { Id = 3
                                 IsVip = false
                                 Credit = 100.0M }
