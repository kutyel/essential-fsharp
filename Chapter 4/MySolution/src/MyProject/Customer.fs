module MyProject.Customer

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

let upgradeCustomer =
    getPurchases
    >> tryPromoteToVip
    >> increaseCreditIfVip
