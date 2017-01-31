module GraphIVR.Core.Models

open System
open System.Text
open System.ComponentModel.DataAnnotations
open System.Collections.Generic
open Neo4jClient.Extension.Cypher


type NodeProperties = 
    {
        id: int
        title: string
        message: string
        retries: int
    }

//type BaseRelationship(fromKey:string, toKey:string, key:string) =
//
//    member x.FromKey = fromKey
//    member x.ToKey = toKey
//    member x.Key = key

//[<CypherLabel(Name = LabelName)>]
type GotoRelationship(fromKey:string, toKey:string, key:string) = 
    inherit BaseRelationship(fromKey, toKey, key)

type SuccessRelationship(fromKey:string, toKey:string, key:string) = 
    inherit BaseRelationship(fromKey, toKey, key)

type FailureRelationship(fromKey:string, toKey:string, key:string) = 
    inherit BaseRelationship(fromKey, toKey, key)

type StartNode = 
    {
        Properties: NodeProperties
    }

type EndNode = 
    {
        Properties: NodeProperties
    }

type Node =
    | START of StartNode
    | END of EndNode
//    | ENTRY of NodeProperties
//    | RETRY of NodeProperties //* RetryProperties
//    | RESPONSE of NodeProperties

type Relationship = 
    | GOTO of GotoRelationship
    | SUCCESS of SuccessRelationship
    | FAIL of FailureRelationship

type Path = 
    | GoToNext of Node * Relationship
    | Failure of Node * Relationship
    | Success of Node * Relationship

    //type RetryProperties =
//    {
//        retries: int
//    }

//type RelationshipProperties = 
//    {
//        
//    }
