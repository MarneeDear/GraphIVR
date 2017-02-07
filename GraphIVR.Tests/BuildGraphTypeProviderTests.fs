namespace GraphIVR.Infrastructure

module BuildGraphTypeProviderTest = 
//    open Haumohio.Neo4j
//    open Neo4jClient
    open System
    open GraphIVR.Core.Models
    open FsUnit.Xunit
    open Xunit
    open Neo4jClient.Cypher

    [<Literal>]
    let ConnectionString = @"http://localhost:7474/db/data"

    type IVRSchema = Haumohio.Neo4j.Schema< ConnectionString >
    let db = new Neo4jClient.GraphClient(Uri(ConnectionString))
    db.Connect()

    [<Fact>]
    let ``Get the START node`` () = 
        let data =
            db.Cypher
                .Match(sprintf "(n:%s)" IVRSchema.Labels.START)
//                .Where( fun n -> n.Id = 1 )
                .Return<IVRSchema.Proxies.START>("(n)")
                .Limit(Nullable<int>(10))
                .Results
        Seq.length data |> should equal 1
        data 
        |> Seq.map(fun x -> int(x.id)) 
        |> Seq.exists(fun x-> x = 1) 
        |> should equal true

    [<Fact>]
    let ``Can get node 1 by Id`` () =
        let data =
            db.Cypher
                .Match("(n)")
                .Where(fun (n:NodeProperties) -> n.id = 1)
                .Return<NodeProperties>("n")
                .Results
        data
        |> Seq.length 
        |> should equal 1

    [<Fact>]
    let ``Can get GOTO from START node to ENTRY customer number`` () =
        let data = 
            db.Cypher
                .Match(sprintf "(s:%s)-[r:%s]->(e:%s)" IVRSchema.Labels.START IVRSchema.Rels.GOTO IVRSchema.Labels.ENTRY)
                .Return(fun (s:ICypherResultItem) (e:ICypherResultItem) -> (s.As<IVRSchema.Proxies.START>(), e.As<IVRSchema.Proxies.ENTRY>()))
                .Results
        let sNode, eNode = data |> Seq.head
        eNode.title |> should equal "CUSTOMER NUMBER"
        sNode.title |> should equal "WELCOME"
        
    [<Fact>]
    let ``Can get SUCCESS and FAIL from CUSTOMER NUMBER to PIN and RETRY CUSTOMER NUMBER no relationship`` () =
        let successPath = 
            db.Cypher
                .Match(sprintf "(e1:%s)-[r:%s]->(e2:%s)" IVRSchema.Labels.ENTRY IVRSchema.Rels.SUCCESS IVRSchema.Labels.ENTRY)
                .Where(fun (e1:NodeProperties) -> e1.title = "CUSTOMER NUMBER")
                .Return(fun (e1:ICypherResultItem) (e2:ICypherResultItem) -> (e1.As<IVRSchema.Proxies.ENTRY>(), e2.As<IVRSchema.Proxies.ENTRY>()))
                .Results
        successPath |> Seq.length |> should equal 1
        let _, e2Node = successPath |> Seq.head
        e2Node.title |> should equal "PIN"
        
        let failurePath = 
            db.Cypher
                .Match(sprintf "(e:%s)-[r:%s]->(re:%s)" IVRSchema.Labels.ENTRY IVRSchema.Rels.FAIL IVRSchema.Labels.RETRY)
                .Where(fun (e:NodeProperties) -> e.title = "CUSTOMER NUMBER")
                .Return(fun (e:ICypherResultItem) (re:ICypherResultItem) -> (e.As<IVRSchema.Proxies.ENTRY>(), re.As<IVRSchema.Proxies.RETRY>()))
                .Results
        failurePath |> Seq.length |> should equal 1
        let _, re = failurePath |> Seq.head
        re.title |> should equal "RETRY CUSTOMER NUMBER"