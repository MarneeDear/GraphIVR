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
                        .With<GraphIVR.Core.Models.StartNode>("START")
                        .Match(fun x -> x.Properties.id)
                        .Merge(fun x -> x.Properties.id)
                        .MergeOnCreate(fun x -> x.Properties.id)
                        .MergeOnMatchOrCreate(fun x -> x.Properties.message)
                        .MergeOnMatchOrCreate(fun x -> x.Properties.retries)
                        .MergeOnMatchOrCreate(fun x -> x.Properties.title)
                        .Set()
    //                    .MergeOnMatchOrCreate

    let EndNode = FluentConfig.Config()
                        .With<GraphIVR.Core.Models.EndNode>("END")
                        .Match(fun x -> x.Properties.id)
                        .Merge(fun x -> x.Properties.id)
                        .MergeOnCreate(fun x -> x.Properties.id)
                        .MergeOnMatchOrCreate(fun x -> x.Properties.message)
                        .MergeOnMatchOrCreate(fun x -> x.Properties.retries)
                        .MergeOnMatchOrCreate(fun x -> x.Properties.title)
                        .Set()

    
    let EntryNode = FluentConfig.Config()
                        .With<GraphIVR.Core.Models.NodeProperties>("ENTRY")
                        .Match(fun x -> x.id)
                        .Merge(fun x -> x.id)
                        .MergeOnCreate(fun x -> x.id)
                        .MergeOnMatchOrCreate(fun x -> x.message)
                        .MergeOnMatchOrCreate(fun x -> x.retries)
                        .MergeOnMatchOrCreate(fun x -> x.title)
                        .Set()


    let RetryNode = FluentConfig.Config()
                        .With<GraphIVR.Core.Models.NodeProperties>("RETRY")
                        .Match(fun x -> x.id)
                        .Merge(fun x -> x.id)
                        .MergeOnCreate(fun x -> x.id)
                        .MergeOnMatchOrCreate(fun x -> x.message)
                        .MergeOnMatchOrCreate(fun x -> x.retries)
                        .MergeOnMatchOrCreate(fun x -> x.title)
                        .Set()


    let ResponseNode = FluentConfig.Config()
                        .With<GraphIVR.Core.Models.NodeProperties>("RESPONSE")
                        .Match(fun x -> x.id)
                        .Merge(fun x -> x.id)
                        .MergeOnCreate(fun x -> x.id)
                        .MergeOnMatchOrCreate(fun x -> x.message)
                        .MergeOnMatchOrCreate(fun x -> x.retries)
                        .MergeOnMatchOrCreate(fun x -> x.title)
                        .Set()

    let GotoRelationship = FluentConfig.Config()
                            .With<GraphIVR.Core.Models.GotoRelationship>("GOTO")
                            .MergeOnMatchOrCreate(fun r -> r.Key)
                            .Set();
                        
    let SuccessRelationship = FluentConfig.Config()
                                .With<GraphIVR.Core.Models.SuccessRelationship>("SUCCESS")
                                .MergeOnMatchOrCreate(fun r -> r.Key)
                                .Set();

    let FailureRelationship = FluentConfig.Config()
                                .With<GraphIVR.Core.Models.FailureRelationship>("FAILURE")
                                .MergeOnMatchOrCreate(fun r -> r.Key)
                                .Set();



    let createNode node = 
        neo4Client.Connect()
        let query = new CypherFluentQuery(neo4Client) :> ICypherFluentQuery
    
        let result = 
            match node with
            | START n -> query.MergeEntity(n)
            | END n -> query.MergeEntity(n)
//            | ENTRY n -> n
//            | RETRY n -> n
//            | RESPONSE n -> n
        //TODO put this in the database
//        query.MergeEntity(result,  //CreateEntity(result, "START").ExecuteWithoutResults()
        ()

    let createRelationship path =
        let n,rel = 
            match path with
            | GoToNext (n1,r) -> (n1,r)
            | Failure (n1,r) -> (n1,r)
            | Success (n1,r) -> (n1,r)

        neo4Client.Connect()

        let query = new CypherFluentQuery(neo4Client) :> ICypherFluentQuery

        let nodeResult =
            match n with 
            | START n -> query.MergeEntity(n) 
            | END n -> query.MergeEntity(n)
//            | ENTRY n -> n
//            | RETRY n -> n
//            | RESPONSE n -> n
//
//        let node2Label = 
//            match n2 with
//            | START -> "START"
//            | END -> "END"
//            | ENTRY -> "ENTRY"
//            | RETRY -> "RETRY"
//            | RESPONSE -> "RESPONSE"

        let relationshipResult =
            match rel with
            | GOTO r -> query.MergeRelationship(r) //TODO relationship class needs to inherit BaseRelationship
            | SUCCESS r -> query.MergeRelationship(r)
            | FAIL r -> query.MergeRelationship(r)
        
//        query.CreateEntity()
    //    ICypherFluentQuery query = new Fl
    //    neo4Client.Cypher
    //        .Merge
//        result |> ignore
        ()