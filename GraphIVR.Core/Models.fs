module GraphIVR.Core.Models

open System
open System.Text
open System.ComponentModel.DataAnnotations
open System.Collections.Generic

type NodeProperties = 
    {
        id: int64
        title: string
        message: string
        retries: int64 option
    }

type RetryProperties =
    {
        retries: int
    }

//type RelationshipProperties = 
//    {
//        
//    }

type Node =
    | START of NodeProperties
    | END of NodeProperties
    | ENTRY of NodeProperties
    | RETRY of NodeProperties //* RetryProperties
    | RESPONSE of NodeProperties

type Relationship = 
    | GOTO
    | SUCCESS
    | FAIL

type Path = 
    | GoToNext of Node * Relationship
    | Failure of Node * Relationship * Node
    | Success of Node * Relationship