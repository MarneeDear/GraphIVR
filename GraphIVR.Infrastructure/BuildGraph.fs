namespace GraphIVR.Infrastructure
module BuildGraph  = 
    open System
    open GraphIVR.Core.Models
    open Neo4jClient
    open Neo4jClient.Cypher
    open FSharp.Configuration

    type Neo4jAppSettings = AppSettings<"App.config">
    [<Literal>]
    let ConnectionString =  @"http://localhost:7474/db/data"
    type IVRSchema = Haumohio.Neo4j.Schema< ConnectionString >
    let neo4jClient = new GraphClient(new Uri(Neo4jAppSettings.ConnectionStrings.Neo4j))

    let nodeLabel node = 
        match node.Label with
            | START -> IVRSchema.Labels.START
            | END -> IVRSchema.Labels.END
            | ENTRY -> IVRSchema.Labels.ENTRY
            | RETRY -> IVRSchema.Labels.RETRY
//            | RESPONSE -> IVRSchema.Labels.RE //NOT yet a part of the schema

    let relationshipType rel =
        match rel with
            | GOTO -> IVRSchema.Rels.GOTO
            | FAIL -> IVRSchema.Rels.FAIL
            | SUCCESS -> IVRSchema.Rels.SUCCESS


    let cypherMergeNode label alias = 
        sprintf "(%s:%s {id: {id}, title: {title}, message: {message}, retries: {retries} })" alias label

    let cypherGetNode node alias = 
        sprintf "(%s:%s)" (nodeLabel node) alias

    let createNode (node:IVRNode) = 

        neo4jClient.Connect()


//            sprintf "(%s:%s {node})" alias label
        let inline (=>) a b = a, box b
        let nodeProperties =  node.Properties
       
        neo4jClient.Cypher
            .Merge(cypherMergeNode (nodeLabel node) "a")
            .OnCreate()
            .Set("a = {nodeProperties}")
            .WithParams(dict [
                            "id" => nodeProperties.id
                            "title" => nodeProperties.title
                            "message" => nodeProperties.message
                            "retries" => nodeProperties.retries
                            "nodeProperties" => nodeProperties
                        ])
          .ExecuteWithoutResults()  

    let deleteNode (node:IVRNode) =
        neo4jClient.Cypher
            .OptionalMatch(sprintf "(n:%s)<-[r]-()" (nodeLabel node)) 
            .Where(fun (n:NodeProperties) -> n.id = node.Properties.id)
            .Delete("r, n") //deletes all incoming relationships
            .ExecuteWithoutResults()

    let createRelationship (path:IVRPath) =
        let node1, rel, node2 = path

        neo4jClient.Cypher
            .Match(cypherGetNode node1 "n1", cypherGetNode node2 "n2")
            .Where(fun (n1:NodeProperties) -> n1.id = node1.Properties.id)
            .AndWhere(fun (n2:NodeProperties) -> n2.id = node2.Properties.id)
            .CreateUnique(sprintf "n1-[:%s]-n2" (relationshipType rel))
            .ExecuteWithoutResults()

                