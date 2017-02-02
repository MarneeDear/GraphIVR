namespace GraphIVR.Infrastructure
module BuildGraph  = 
    open System
    open GraphIVR.Core.Models
    open Neo4jClient
    open Neo4jClient.Cypher
    open FSharp.Configuration

    type Neo4jAppSettings = AppSettings<"App.config">

    let neo4Client = new GraphClient(new Uri(Neo4jAppSettings.ConnectionStrings.Neo4j), "neo4j", "CV1g:[dluwfX");

    let createNode (node:IVRNode) = 
        let label = 
            match node.Label with
                | START -> "START"
                | END -> "END"
                | ENTRY -> "ENTRY"
                | RETRY -> "RETRY"
                | RESPONSE -> "RESPONSE"

        neo4Client.Connect()

        let cypherNode label alias = 
            sprintf "(%s:%s {id: {id}, title: {title}, message: {message}, retries: {retries} })" alias label

//            sprintf "(%s:%s {node})" alias label
        let inline (=>) a b = a, box b
        let nodeProperties =  node.Properties
       
        neo4Client.Cypher
            .Merge(cypherNode label "a")
            .OnCreate()
            .Set("a = {nodeProperties}")
            .WithParams(dict [
                            "id" => nodeProperties.Id
                            "title" => nodeProperties.Title
                            "message" => nodeProperties.Message
                            "retries" => nodeProperties.Retries
                            "nodeProperties" => nodeProperties
                        ])
          .ExecuteWithoutResults()  
        

                