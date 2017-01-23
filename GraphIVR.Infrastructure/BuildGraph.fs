module BuildGraph

open GraphIVR.Core.Models

let createNode node =     
    let result = 
        match node with
        | START n -> n
        | END n -> n
        | ENTRY n -> n
        | RETRY n -> n
        | RESPONSE n -> n
    result |> ignore