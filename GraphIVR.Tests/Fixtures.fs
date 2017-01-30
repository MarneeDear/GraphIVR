namespace GraphIVR.Tests
open System

module Fixtures = 
    type BuildGraphsFixture() =
        //DO THE TEST SETUP HERE
        //(THIS IS THE CONSTRUCTOR)
        do sprintf "SETUP THE GRAPH." |> ignore
        interface IDisposable with
        
            member __.Dispose () =
                //CLEAN UP TEST DATA OR WHATEVER YOU NEED TO CLEANUP YOUR TESTS
                ()

//TRYING THINGS OUT HERE
//    let SetupDatabaseFixture = 
//        ()
//
//    let CleanupDatabaseFixture =
//        ()
//
//module TestRunner =
//    open Fixtures
//    
//    let RunBlogTests test = 
//        SetupDatabaseFixture
//        test //TODO not runnint the tests!
//        CleanupDatabaseFixture
//        ()
