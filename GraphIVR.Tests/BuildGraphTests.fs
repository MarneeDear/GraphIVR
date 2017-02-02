module BuildGraphTests

open System
open GraphIVR.Core.Models
open GraphIVR.Infrastructure
open Xunit
open FsUnit.Xunit

[<Fact>]
let ``Can create a START node`` () =
    let node = 
        {
            Label = Label.START
            Properties = 
                {
                    Id = 78878
                    Title = "TEST"
                    Message = "TEST"
                    Retries = 0
                }
        }
    BuildGraph.createNode(node)