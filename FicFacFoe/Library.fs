module FicFacFoe

type Player = X | O with
  static member other = function X -> O | O -> X

type Board = Player option seq seq

let mkBoard n =
  Seq.replicate n None
  |> Seq.replicate n


let play (p: Player) (i,j) (board: Board) =
  let row = board |> Seq.item i

  match row |> Seq.item j with
  | Some p -> Error $"can't play at ({i},{j}): {p} already did"
  | None -> board |> Seq.updateAt i (row |> Seq.updateAt j (Some p)) |> Ok

let play' (p: Player) (i,j) =
   Result.bind (play p (i,j))

let diagonal = Seq.mapi Seq.item

let counterDiagonal = Seq.map Seq.rev >> diagonal

let winner (board: Board) =
  seq {
    yield! board
    yield! (board |> Seq.transpose)
    diagonal board
    counterDiagonal board
  }
  |> Seq.choose (Seq.distinct >> Seq.tryExactlyOne)
  |> Seq.tryExactlyOne
  |> Option.flatten
