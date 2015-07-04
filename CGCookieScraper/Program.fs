// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open OpenQA.Selenium
open OpenQA.Selenium.Support.UI

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

goto "https://cgcookiearchive.com/"
findWID "header-login-form-toggle" |> click
setText (findWID "user_login") username
setText (findWID "user_pass")  password
click (findWID "wp-submit")
exit 0 // return an integer exit code
