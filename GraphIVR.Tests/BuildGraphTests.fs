namespace GraphIVR.Tests
open Xunit
open System
open FsUnit.Xunit
open GraphIVR.Infrastructure
open GraphIVR.Core.Models


module BuildGraphTests =
    type BuildGraphsTests() =
        [<Fact>]
        member __.``Can create a start node`` () =
            //DO THE TEST STUFF
//            "START NODE" |> should equal "START NODE"
            let node =
                START(
                    {
                        id = 99
                        title = "TEST"
                        message = "TEST TEST TEST" 
                        retries = 0
                    }
                )
            
            BuildGraph.createNode(node)
        interface IClassFixture<Fixtures.BuildGraphsFixture>

