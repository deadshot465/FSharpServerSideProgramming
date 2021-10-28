namespace ServerSideProgramming.Controllers

open System
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

type HelloController (logger: ILogger<HelloController>) =
    inherit Controller()
    let defaultId = "ECC"
    let defaultPassword = 0144
    
    member this.Index () =
        this.ViewData.["message"] <- "Welcome to ECC."
        this.View()
        
    member private this.validateId id =
        if System.String.IsNullOrWhiteSpace id || id <> defaultId then
            "IDが存在していません。"
        else
            ""
            
    member private this.validatePassword password =
        let pwd = ref 0
        if System.String.IsNullOrWhiteSpace password || (System.Int32.TryParse (password, pwd) |> not) then
            "パスワードが間違っています。"
        else
            if pwd.contents <> defaultPassword then
                "パスワードが間違っています。"
            else
                ""
    
    [<HttpGet("hello/form/{timeout?}")>]
    member this.Form (timeout: bool) =
        if timeout then
            this.ViewData.["timeout"] <- "Timeout"
            this.View()
        else
            this.ViewData.["timeout"] <- ""
            this.HttpContext.Session.SetString("access_time", DateTime.Now.ToString())
            this.View("Form")
    
    [<HttpPost>]    
    member this.Form (id: string, password: string) =
        let accessTime = this.HttpContext.Session.GetString "access_time"
        if String.IsNullOrWhiteSpace accessTime then
            this.Form true
        else
            let validateIdResult = this.validateId id
            let validatePasswordResult = this.validatePassword password
            this.ViewData.["idValidateMsg"] <- validateIdResult
            this.ViewData.["passwordValidateMsg"] <- validatePasswordResult
            if (String.IsNullOrWhiteSpace validateIdResult |> not) || (String.IsNullOrWhiteSpace validatePasswordResult |> not) then
                this.View("Form")
            else
                this.View("Index")
            
    [<HttpGet("hello/session/{id?}/{name?}")>]
    member this.Session (id: int, name: string) =
        this.ViewData.["message"] <- "セッションにIDとNameを保存しました。"
        this.HttpContext.Session.SetInt32("id", id)
        let name = if name = null then "" else name
        this.HttpContext.Session.SetString("name", name)
        this.View()
        
    [<HttpGet>]
    member this.SessionCheck (id: int, name: string) =
        this.ViewData.["message"] <- "保存されているセッションの値を確認します。"
        this.ViewData.["id"] <- this.HttpContext.Session.GetInt32("id")
        this.ViewData.["name"] <- this.HttpContext.Session.GetString("name")
        this.View("Session")