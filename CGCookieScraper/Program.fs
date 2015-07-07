// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open OpenQA.Selenium
open OpenQA.Selenium.Support.UI
open System.Net
open System.IO
open System

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

let findWT tag = wait.Until(ExpectedConditions.ElementExists(By.TagName(tag)))

let findWLX xpath = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath(xpath)))

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
                |> List.filter (fun l -> match l with | Link(url, name) -> url.ToLower().Contains("cgcookie") | _ -> false)
                |> List.map(fun l -> match l with | Link(url,name) -> Link(url.ToLower().Replace("cgcookie.com", "cgcookiearchive.com"), name))
    let folders = elements 
                  |> Seq.filter(fun e -> e.FindElement(By.XPath("./*[1]")).TagName = "h3")
                  |> Seq.toList
                  |> List.map(fun f -> Container(f.FindElement(By.XPath("./*[1]")).Text, parseContainer (f.FindElements(By.XPath("./dl/dt")))))
                  |> List.filter(fun c -> match c with | Container(name, nodes) -> Seq.length  nodes > 0 | _ -> false)
    List.append folders links

let rec fout = System.IO.File.CreateText("out.txt")
let rec printTree tree prefix =
    match tree with
    | Container(name, nodes) -> fprintf fout "%s-->%s\n" prefix name; nodes |> List.iter (fun n -> printTree n (prefix + "-->" + name))
    | Link(url, name) -> fprintf fout "%s--:%s (%s)\n" prefix name url

let clientDownload (url:string) extension=
    let client = new WebClient()
    let header = (driver.Manage().Cookies.AllCookies) |> Seq.fold (fun acum s -> acum + s.Name + "=" + s.Value + ";") ""
    client.Headers.Add(HttpRequestHeader.Cookie, header)
    let agent = string (driver.ExecuteScript("return navigator.userAgent"))
    client.Headers.Add("user-agent", agent)
    let stream = client.OpenRead(url)
    let starti = 
        if (url.Contains("/post_id")) then url.Substring(0, url.IndexOf("/?post_id")).LastIndexOf("/") + 1
        else 0
    let endi = url.IndexOf("/?post_id")
    let len = endi - starti
    let filename = 
        if len > 0 then url.Substring(starti, len) + extension
        else url.Substring(url.LastIndexOf("/") + 1)
    use fout = System.IO.File.Create(System.IO.Path.Combine(System.Environment.CurrentDirectory, filename))
    stream.CopyTo(fout)

let downloadVideo () = 
    try
        //wait.Until(ExpectedConditions.ElementExists(By.ClassName("post-downloads-toggle"))).Click()
        driver.FindElement(By.ClassName("post-downloads-toggle")).Click()
        let videoLink = findWLX "//div[@class='post-downloads']/a" |> Seq.filter (fun e -> e.Text.Trim() = "HD Video")
        if Seq.length videoLink > 0 then
            videoLink |> Seq.iteri(fun i a -> clientDownload (a.GetAttribute("href")) ".zip")
        else
            failwith "Video not found"
    with
        _ -> try clientDownload ((findWT "source").GetAttribute("src")) ".mp4" with _ -> ()

let downloadNoneVideoFiles () =
    try
        //wait.Until(ExpectedConditions.ElementExists(By.ClassName("post-downloads-toggle"))).Click()
        driver.FindElement(By.ClassName("post-downloads-toggle")).Click()
        let fileLink = findWLX "//div[@class='post-downloads']/a" |> Seq.filter (fun e -> e.Text.Trim() <> "HD Video")
        fileLink |> Seq.iter (fun e -> printfn "%s" e.Text)
        if Seq.length fileLink > 0 then
            fileLink |> Seq.iter (fun a -> printfn "Saving"; clientDownload (a.GetAttribute("href")) "_files.zip"; printfn "Saved")
    with _ -> ()

let rec saveNodesToDisk path container =
    match container with 
    | Container(name, links) -> //Create direcotries and then save all the links data
                                let newPath = System.IO.Path.Combine(path, name)
                                System.IO.Directory.CreateDirectory(newPath) |> ignore
                                links |> List.iter (fun l -> match l with | Link(_,_) -> saveNodesToDisk newPath l | _ -> ())
                                links |> List.filter (fun l -> match l with | Container(_,_) -> true | _ -> false) |> List.iter (saveNodesToDisk newPath)
    | Link(url, name) -> //Save all the links data 
                         File.CreateText(Path.Combine(path, name)) |> ignore

//Get all the links and file structure we will need for the export
if (not (System.IO.File.Exists("bookmarks.html"))) then
    printfn "You must have a bookmarks.html in the working directory. See the readme."
    exit 1
goto ("file:///" + (System.IO.Path.Combine(System.Environment.CurrentDirectory, "bookmarks.html")))
let eles = findsWX "/html/body/dl/dt"
printfn "Count: %d" (Seq.length eles)
let result = Container("Output", parseContainer eles)
printTree result ""

//Log in
goto "https://cgcookiearchive.com/"
findWID "header-login-form-toggle" |> click
setText (findWID "user_login") username
setText (findWID "user_pass")  password
click (findWID "wp-submit")

//Save all links with correct file structure to disc
if (not (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "Output")))) then
    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Output")) |> ignore

saveNodesToDisk (Path.Combine(Environment.CurrentDirectory)) result

//goto "https://cgcookiearchive.com/blender/lessons/4-4-4-exercise-throwing-ball/"
//goto "https://cgcookiearchive.com/concept/lessons/1-software-tablets/"
//goto "https://cgcookiearchive.com/unity/cgc-courses/crash-course-breakout-particles-mini-course/"
//downloadVideo ()

//Download all content related to our links
//TODO: The actual downloading.

System.Console.ReadLine() |> ignore
driver.Close()
exit 0 // return an integer exit code
