open System
open System.IO
open System.Diagnostics
open System.Text.Json

open Microsoft.Data.Analysis

let readFile (fname : string) : string option = 
    if File.Exists fname then
        Some (File.ReadAllText fname)
    else
        None

let parseJson (json : string) : JsonElement option =
    try
        Some (JsonDocument.Parse(json).RootElement)
    with
        _ -> None

let printUsage () =
  printfn "In usage message"
  0

let parseCli args =
    match args with
    | "-v" :: _ ->
      printUsage()
    | _ -> 0

[<EntryPoint>]    // This is the attribute syntax in F#
let main argv =
    argv |> List.ofArray |> parseCli |> ignore
    let proc = Process.GetCurrentProcess()
    let fcontent = readFile (Path.Combine (Environment.GetEnvironmentVariable("HOME"), ".bashrc"))
    let readChar = match fcontent with
                   | Some x -> x.Length
                   | None -> 0
    printfn "Hello (%d) from process (%s) in F#" readChar proc.ProcessName
    0
