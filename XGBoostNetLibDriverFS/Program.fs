open System
//open System.Reflection
open System.IO
open System.Diagnostics
open System.Text.Json

open Microsoft.Data.Analysis

let readFile (fname : string) : string option = 
    if File.Exists fname then
        Some (File.ReadAllText fname)
    else
        None

let readCsvFile (fname : string) =
    if File.Exists fname then
        Some (DataFrame.LoadCsv(fname))
    else
        None

// let parseJson (json : string) : JsonElement option =
//     try
//         Some (JsonDocument.Parse(json).RootElement)
//     with
//         _ -> None

// let printUsage () =
//   printfn "In usage message"
//   0

// let parseCli args =
//     match args with
//     | "-v" :: _ ->
//       printUsage()
//     | _ -> 0

// [<EntryPoint>]    // This is the attribute syntax in F#
// let main argv =
//     argv |> List.ofArray |> parseCli |> ignore
//     let proc = Process.GetCurrentProcess()
//     let fcontent = readFile (Path.Combine (Environment.GetEnvironmentVariable("HOME"), ".bashrc"))
//     let readChar = match fcontent with
//                    | Some x -> x.Length
//                    | None -> 0
//     printfn "Hello (%d) from process (%s) in F#" readChar proc.ProcessName
//     0


[<EntryPoint>]    // This is the attribute syntax in F#
let main argv =
  let proc = Process.GetCurrentProcess()
  let nproc = Environment.ProcessorCount
  let test = async {                                       
    printfn "Loading data!"                   
    System.Threading.Thread.Sleep(500)
    printfn "Loaded Data!"
  }                  
  test |> Async.Start
  printfn "Running as process %d on a %d-core count machine" proc.Id nproc

  //let datafname = Path.Combine(Assembly.GetExecutingAssembly().Location, "data", "boston_housing.csv")
  let datafname = Path.Combine(AppContext.BaseDirectory, "data", "boston_housing.csv")

  printfn "Checking for file %s..." datafname 
  if File.Exists(datafname) then
    printfn "the file exists"
  else
    printfn "couldnt' find the file"

  Console.ReadLine() |> ignore
  0