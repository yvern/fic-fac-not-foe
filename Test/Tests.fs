module Test

open NUnit.Framework
open Swensen.Unquote

open FicFacFoe


[<Test>]
let ``start off with empty (nobody) board`` () =
    test <@
      mkBoard 3
      |> Seq.forall (Seq.forall Option.isNone)
      @>

[<Test>]
let ``X won diagonal`` () =
    test <@
      mkBoard 3
      |> play X (0,0)
      |> Result.bind (play O (1,0))
      |> Result.bind (play X (1,1))
      |> Result.bind (play O (2,0))
      |> Result.bind (play X (2,2))
      |> Result.toOption
      |> Option.bind winner      |> Option.contains X
      @>

[<Test>]
let ``O won diagonal`` () =
    test <@
      mkBoard 3
      |> play X (0,0)
      |> Result.bind (play O (2,0))
      |> Result.bind (play X (1,0))
      |> Result.bind (play O (0,2))
      |> Result.bind (play X (2,2))
      |> Result.bind (play O (1,1))
      |> Result.toOption
      |> Option.bind winner
      |> Option.contains O
      @>

[<Test>]
let ``X won first row`` () =
    test <@
      mkBoard 3
      |> play X (0,0)
      |> Result.bind (play O (2,0))
      |> Result.bind (play X (0,1))
      |> Result.bind (play O (2,2))
      |> Result.bind (play X (0,2))
      |> Result.bind (play O (1,1))
      |> Result.toOption
      |> Option.bind winner
      |> Option.contains X
      @>

[<Test>]
let ``X won first col`` () =
    test <@
      mkBoard 3
      |> play X (0,0)
      |> Result.bind (play O (0,2))
      |> Result.bind (play X (1,0))
      |> Result.bind (play O (2,2))
      |> Result.bind (play X (2,0))
      |> Result.bind (play O (1,1))
      |> Result.toOption
      |> Option.bind winner
      |> Option.contains X
      @>


[<Test>]
let ``can't repeat play`` () =
    test <@
      mkBoard 3
      |> play X (0,0)
      |> Result.bind (play O (0,0))
      |> Result.isError
      @>
