open System
open System.IO

let readFile (fname : string) : string option = 
    if File.Exists fname then
        Some (File.ReadAllText fname)
    else
        None

[<EntryPoint>]    // This is the attribute syntax in F#
let main argv =
    let fcontent = readFile (Path.Combine (Environment.GetEnvironmentVariable("HOME"), ".bashrc"))
    let readChar = match fcontent with
                   | Some x -> x.Length
                   | None -> 0
    printfn "Hello (%d) from F#" readChar
    0
