[<AutoOpen>]
module GraphIVR.Core.Rop

open System

// todo: move this module into the COM.Core project

type RopResult<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of 'TFailure

type ResultInterop<'TSuccess, 'TFailure> = {
    IsSuccess : bool
    Success : 'TSuccess
    Failure : 'TFailure
}

///Use this to unwrap the RopResult into something easier to use in C#
let toResultInterop result =
    match result with
    | Success s -> { IsSuccess=true; Success=s; Failure=Unchecked.defaultof<_> }
    | Failure f -> { IsSuccess=false; Success=Unchecked.defaultof<_>; Failure=f }


//with 
//    member x.Match(success: Func<_,_>, failure: Func<_,_>) =
//        match x with
//        | Success r -> success.Invoke r
//        | Failure r -> failure.Invoke r

// convert a single value into a two-track result
let succeed x = 
    Success x

// convert a single value into a two-track result
let fail x = 
    Failure x

// apply either a success function or failure function
let either successFunc failureFunc ropInput =
    match ropInput with
    | Success s -> successFunc s
    | Failure f -> failureFunc f

// Apply a function if the input is success, otherwise fail
let bind f = either f fail

// Apply a standard function if the input is success
let map f = either (f >> succeed) fail

/// Try to apply a function, if an exception is thrown return a Failure
let tryCatch f x =
    try
        f x |> Success
    with
    | ex -> Failure ex.Message

/// given an RopResult, call a unit function on the success branch
/// and pass thru the result
let successTee f result = 
    let fSuccess x = 
        f x
        Success x

    let fFailure errs = fail errs 

    either fSuccess fFailure result

/// If ropInput is a success return a success with the given value otherwise fail
let succeedWith value ropInput =
    match ropInput with
    | Success _ -> succeed value
    | Failure msg -> fail msg

let isFailure = function
    | Failure _ -> true
    | Success _ -> false

let unwrapSuccess = function
    | Failure _ -> failwith "Cannot unwrap a success result if it is a failure"
    | Success x -> x

/// Collect on the successfull results in ropResults or return the first failure
let collectSuccess ropResults = 
    let firstFailure = ropResults |> List.tryFind isFailure
    match firstFailure with
    | Some failure -> failure
    | None ->         Success (ropResults |> List.collect unwrapSuccess)

/// Iterate over a list, short circuiting on the first failure
let rec ropListIter f lst =     
    match lst with 
    | [] -> Success ()
    | x::xs ->
        match (f x) with
        | Failure msg -> fail msg
        | Success _ -> ropListIter f xs


