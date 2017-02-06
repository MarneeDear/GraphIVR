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
                    id = 78878
                    title = "TEST"
                    message = "TEST"
                    retries = 0
                }
        }
    BuildGraph.createNode(node)
    BuildGraph.deleteNode(node)