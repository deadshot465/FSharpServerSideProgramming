namespace ServerSideProgramming.Controllers

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
    
    member this.Form () =
        this.View("Form")
    
    [<HttpPost>]    
    member this.Form (id: string, password: string) =
        let validateIdResult = this.validateId id
        let validatePasswordResult = this.validatePassword password
        this.ViewData.["idValidateMsg"] <- validateIdResult
        this.ViewData.["passwordValidateMsg"] <- validatePasswordResult
        if (System.String.IsNullOrWhiteSpace validateIdResult |> not) || (System.String.IsNullOrWhiteSpace validatePasswordResult |> not) then
            this.View("Form")
        else
            this.View("Index")