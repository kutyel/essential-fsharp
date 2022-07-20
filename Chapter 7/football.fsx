type Score = int * int

let (|CorrectScore|_|) (exp: Score, act: Score) = if exp = act then Some() else None

let (|Draw|HomeWin|AwayWin|) (score: Score) =
    match score with
    | (h, a) when h = a -> Draw
    | (h, a) when h > a -> HomeWin
    | _ -> AwayWin

let (|CorrectResult|_|) (expected: Score, actual: Score) =
    match (expected, actual) with
    | (Draw, Draw)
    | (HomeWin, HomeWin)
    | (AwayWin, AwayWin) -> Some()
    | _ -> None

let goalsScore (expected: Score) (actual: Score) =
    let home = [ fst expected; fst actual ] |> List.min
    let away = [ snd expected; snd actual ] |> List.min
    (home * 15) + (away * 20)

let resultsScore (expected: Score) (actual: Score) =
    match (expected, actual) with
    | CorrectScore -> 400
    | CorrectResult -> 100
    | _ -> 0

let calculatePoints (expected: Score) (actual: Score) =
    [ goalsScore; resultsScore ]
    |> List.sumBy (fun f -> f expected actual)

let assertnoScoreDrawCorrect = calculatePoints (0, 0) (0, 0) = 400
let assertHomeWinExactMatch = calculatePoints (3, 2) (3, 2) = 485
let assertHomeWin = calculatePoints (5, 1) (4, 3) = 180
let assertIncorrect = calculatePoints (2, 1) (0, 7) = 20
let assertDraw = calculatePoints (2, 2) (3, 3) = 170
