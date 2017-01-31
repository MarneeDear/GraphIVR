namespace GraphIVR.Tests
open Xunit
open System
open FsUnit.Xunit
open GraphIVR.Infrastructure
open GraphIVR.Core.Models


module BuildGraphTestsIClassFixture =
    type BuildGraphsTests() =
        [<Fact>]
        member __.``Can create a start node`` () =
            let node =
                START(
                    {
                        Properties.id = 99
                        title = "TEST"
                        message = "TEST TEST TEST" 
                        retries = 0
                    }
                )
            
            BuildGraph.createNode(node)
        interface IClassFixture<Fixtures.BuildGraphsFixture>

module BuildGraphTestsFunctional =
    [<Fact>]
    let ``Can create a start node`` () =
        
    

//TRYING THINGS OUT HERE
//module BlogTests =
//    open TestRunner
//
//    let SetupDatabaseFixture = 
//        ()
//
//    
//    let test1 () =
//        "HELLO" |> should equal "HELLO"
//        
//
//    let test2 () =
//        "HELLO" |> should not' (equal "GOODBYE")
//
//    [<Fact>]
//    let ``We can compose these tests`` () =
////        let tests x =
////            (>>) test1 test2 x
//            
//        RunBlogTests (test1 ()) |> ignore

//Example of setup and teardown with transactions that you can rollback
//we can commit or rollback tranactions with neo4jclient
//https://github.com/Readify/Neo4jClient/wiki/Transactions
//[<Test>]
//let ``should get address history from SSN number`` () =
//    let ssn = "196403063374"
//
//    // setup
//    db.Connection.Open()
//    let transaction = db.Connection.BeginTransaction(isolationLevel = IsolationLevel.Serializable)
//    db.DataContext.Transaction <- transaction
//
//    db.SetupTestData() |> ignore // <-- here the db is prepped with test data
//
//    try
//        // act
//        let addresses = getAddressHistory ssn |> Seq.toList
//
//        // assert
//        addresses.Length |> should equal 2
//        
//    finally
//        // teardown
//        transaction.Rollback()
//        db.Connection.Close()