open System.IO

type Tree<'T> =
    | Branch of 'T * Tree<'T> seq
    | Leaf of 'T

type Connection = {
    Start: string
    Finish: string
    Distance: int
}

type Waypoint = {
    Location: string
    Route: string
    TotalDistance: int
}

let getUnvisited connections current =
    connections
    |> List.filter (fun cn -> cn.Route |> List.exists (fun loc -> loc = cn.Finish) |> not)
    |> List.map (fun cn -> {
        Location= cn.Finish
        Route= cn.Start :: current.Route
        TotalDistance= cn.Distance + current.TotalDistance
    })

let rec treeToList tree =
    match tree with
    | Leaf x -> [x]
    | Branch (_, xs) -> List.collect treeToList (Seq.toList xs)

let findPossibleRoutes start finish (routeMap: Map<string, Connection list>) =
    let rec go current =
        let nextRoutes = getUnvisited routeMap[current.Location] current
        if nextRoutes |> List.isEmpty |> not && current.Location <> finish then
            Branch (current, seq { for next in nextRoutes do go next })  
        else
            Leaf current
    go { Location=start; Route=[]; TotalDistance=0 }
    |> treeToList
    |> List.filter (fun wp -> wp.Location = finish)

let selectShortestRoute =
    List.minBy (fun wp -> wp.TotalDistance)
    >> fun wp -> wp.Location :: wp.Route |> List.rev, wp.TotalDistance 

let loadData path =
    path
    |> File.ReadLines
    |> Seq.skip 1
    |> fun rows ->
        for row in rows do
            match row.Split(",") with
            | [|start;finish;distance] ->
                { Start=start; Finish=finish; Distance=distance }
                { Start=finish; Finish=start; Distance=distance }
            | _ -> failwith "Row is badly formed"
    |> List.groupBy (fun cn -> cn.Start)
    |> Map.ofList

let run start finish =
    Path.Combine(__SOURCE_DIRECTORY__, "resources", "data.csv")
    |> loadData
    |> findPossibleRoutes start finish
    |> selectShortestRoute
    |> prinfn "%A"

let result = run "Cogburg" "Leverstorm"
