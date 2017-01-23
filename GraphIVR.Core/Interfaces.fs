module GraphIVR.Core.Interfaces

open System

type ILogger =
    abstract member InfoMsg: string -> unit
    abstract member Error: string -> Exception -> unit
    abstract member ErrorMsg: string -> unit
