namespace GraphIVR.Infrastructure
module BuildGraph =

    open GraphIVR.Core.Models
    open Neo4jClient
    open Neo4jClient.Cypher
    open Neo4jClient.Extension.Cypher
    open System
    open FSharp.Configuration


    type Neo4jAppSettings = AppSettings<"App.config">

    let neo4Client = new GraphClient(new Uri(Neo4jAppSettings.ConnectionStrings.Neo4j), "neo4j", "CV1g:[dluwfX");


    let StartNode = FluentConfig.Config()
                        .With<GraphIVR.Core.Models.NodeProperties>("START")
                        .Match(fun x -> x.id)
                        .Merge(fun x -> x.id)
                        .MergeOnCreate(fun x -> x.id)
                        .MergeOnMatchOrCreate(fun x -> x.message)
                        .MergeOnMatchOrCreate(fun x -> x.retries)
                        .MergeOnMatchOrCreate(fun x -> x.title)
                        .Set()
    //                    .MergeOnMatchOrCreate


    let createNode node =     
        let result = 
            match node with
            | START n -> n
            | END n -> n
            | ENTRY n -> n
            | RETRY n -> n
            | RESPONSE n -> n
        //TODO put this in the database
        neo4Client.Connect()
        let query = new CypherFluentQuery(neo4Client) :> ICypherFluentQuery
        query.CreateEntity(result, "START").ExecuteWithoutResults()
    //    ICypherFluentQuery query = new Fl
    //    neo4Client.Cypher
    //        .Merge
        result |> ignore