// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open OpenQA.Selenium
open OpenQA.Selenium.Support.UI

type InputNodes = 
    | Link of URL:string * Name:string
    | Container of Name:string * Elements:InputNodes list

let driver = new OpenQA.Selenium.Firefox.FirefoxDriver()

let wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10.))

if (not (System.IO.File.Exists("credentials.txt"))) then
    printfn "You need a credentials.txt with your username and password on the first 2 lines."
    exit 1
let username,password = 
    let fin = System.IO.File.ReadAllLines("credentials.txt")
    (fin.[0], fin.[1])

let goto (url:string) = driver.Navigate().GoToUrl(url)

let findID id = driver.FindElementById(id)

let click (element:IWebElement) = element.Click()

let setText (element:IWebElement) text = element.SendKeys(text)

let findWID id = wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)))

let findsWT tag = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.TagName(tag)))

let findsWX xpath = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath(xpath)))

let text (element:IWebElement) = element.Text

let rec parseContainer (elements:IWebElement seq) =
    let links = 
                let toLink (e:IWebElement) = 
                    Link(e.FindElement(By.XPath("./*[1]")).GetAttribute("href"), e.FindElement(By.XPath("./*[1]")).Text)
                elements
                |> Seq.filter(fun e -> e.FindElement(By.XPath("./*[1]")).TagName = "a") 
                |> Seq.toList 
                |> List.map(fun e -> toLink e)
    let folders = elements 
                  |> Seq.filter(fun e -> e.FindElement(By.XPath("./*[1]")).TagName = "h3")
                  |> Seq.toList
                  |> List.map(fun f -> Container(f.FindElement(By.XPath("./*[1]")).Text, parseContainer (f.FindElements(By.XPath("./dl/dt")))))
    List.append folders links
    (*
    for ele in elements do
        let firstChild = ele.FindElement(By.XPath("./*[1]"))
        if firstChild.TagName = "h3" then
            printfn "Container found: %s->%s" name (firstChild.Text)
            Container(firstChild.Text, parseContainer firstChild.Text (ele.FindElements(By.XPath("./dl/dt"))))
        else
            printfn "Link found: %s->%s" name firstChild.Text
            Link(firstChild.GetAttribute("href"), firstChild.Text)
            *)

let rec printTree tree prefix =
    match tree with
    | Container(name, nodes) -> printfn "%s-->%s" prefix name; nodes |> List.iter (fun n -> printTree n (prefix + "-->" + name))
    | Link(url, name) -> printfn "%s--:%s (%s)" prefix name url
(*
goto "https://cgcookiearchive.com/"
findWID "header-login-form-toggle" |> click
setText (findWID "user_login") username
setText (findWID "user_pass")  password
click (findWID "wp-submit")
*)
goto ("file:///" + (System.IO.Path.Combine(System.Environment.CurrentDirectory, "bookmarks.html")))
let eles = findsWX "/html/body/dl/dt"
printfn "Count: %d" (Seq.length eles)
eles |> Seq.iter (fun e -> printfn "%s" (text e))
let result = Container("Root", parseContainer eles)
printTree result ""
//driver.ExecuteScript("return document.documentElement.outerHTML")

//goto "file:///C:/Users/David/OneDrive/Documents/Works/fsharp/CGCookieArchiver/CGCookieScraper/bin/Debug/bookmarks.html"

System.Console.ReadLine() |> ignore
driver.Close()
exit 0 // return an integer exit code
