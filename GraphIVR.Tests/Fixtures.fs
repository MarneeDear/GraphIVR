namespace GraphIVR.Tests
open System

module Fixtures = 
    type BuildGraphsFixture() =
        //DO THE TEST SETUP HERE
        //(THIS IS THE CONSTRUCTOR)
        do sprintf "Hey man do somehting already." |> ignore
        interface IDisposable with
        
            member __.Dispose () =
                //CLEAN UP TEST DATA OR WHATEVER YOU NEED TO CLEANUP YOUR TESTS
                ()


