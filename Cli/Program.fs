open System
open FicFacFoe

let showCell =
    Option.map (sprintf "%A")
    >> Option.defaultValue "."

let showLine = Seq.map showCell >> String.concat " "

let showBoard =
    Seq.map showLine
    >> String.concat "\n  "
    >> printfn "  %s"

let parseInt (s: string) =
    match Int32.TryParse(s) with
    | (ok, i) when ok -> Ok i
    | _ -> Error $"'{s}' is not a valid integer"

let readLine () =
    match Console.ReadLine().Split(",") |> Array.map _.Trim() with
    | [|i ; j|] -> Ok (i,j)
    | line -> Error $"this game is 2D and you should give 2 comma separated coordinates instead of {line.Length}"

let readPlay (i,j) =
    match (parseInt i, parseInt j) with
    | Ok ic, Ok jc -> Ok (ic,jc)
    | e -> sprintf "invalid coordinates: %A" e |> Error

let initBoard =
    Array.tryHead
    >> Option.fold (fun _ -> parseInt) (Ok 3)
    >> Result.map mkBoard

let runTurn turn board p  =
    play turn p board
    |> Result.map (fun nb -> showBoard nb; nb, winner nb)


[<TailCall>]
let rec gameLoop turn board =
    readLine ()
    |> Result.bind readPlay
    |> Result.bind (runTurn turn board)
    |> function
        | Ok (_, Some player) -> printfn "%A won!" player
        | Ok (b, None) -> gameLoop (Player.other turn) b
        | Error e -> printfn $"ERROR: {e}.\n please replay"; gameLoop turn board

[<EntryPoint>]
let main args =
    match initBoard args with
    | Ok b -> showBoard b ; gameLoop X b ; 0
    | Error msg -> printfn "ERROR: %s.\n exiting" msg; 1
