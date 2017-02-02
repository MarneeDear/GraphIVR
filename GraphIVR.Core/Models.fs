module GraphIVR.Core.Models

type Label = 
    | START
    | END 
    | ENTRY 
    | RETRY 
    | RESPONSE 

type NodeProperties = 
    {
        Id: int
        Title: string
        Message: string
        Retries: int

    }

type IVRNode = 
    {
        Label: Label 
        Properties: NodeProperties
    }

type Relationship = 
    | GOTO 
    | SUCCESS 
    | FAIL 

type Path = IVRNode * Relationship * IVRNode


