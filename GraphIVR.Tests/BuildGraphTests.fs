namespace GraphIVR.Tests
open Xunit
open System
open FsUnit.Xunit

module BuildGraphTests =
    type BuildGraphsTests() =
        [<Fact>]
        member __.``Can create a start node`` () =
            //DO THE TEST STUFF
            "START NODE" |> should equal "START NODE"
        interface IClassFixture<Fixtures.BuildGraphsFixture>

