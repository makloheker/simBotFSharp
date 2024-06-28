// fakyu
open System
open System.Net.Http
open System.Text
open System.Threading.Tasks
open Newtonsoft.Json.Linq

let apiUrl = "https://api.simsimi.vn/v1/simtalk"

let sendRequest (text: string) =
    async {
        use client = new HttpClient()
        let content = new StringContent($"text={text}&lc=id", Encoding.UTF8, "application/x-www-form-urlencoded")
        let! response = client.PostAsync(apiUrl, content) |> Async.AwaitTask
        let! responseString = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        let jsonResponse = JObject.Parse(responseString)
        return jsonResponse.["message"].ToString()
    }

let rec botLoop () =
    async {
        Console.Write("you>: ")
        let userInput = Console.ReadLine()
        if userInput = "exit" then
            Console.WriteLine("dahh...")
        else
            let! responseMessage = sendRequest userInput
            Console.WriteLine($"bot>: {responseMessage}")
            return! botLoop()
    }

[<EntryPoint>]
let main argv =
    botLoop() |> Async.RunSynchronously
    0
