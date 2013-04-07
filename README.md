scriptcs-webapi
===============

# Web API Script Pack

## What is it?
Makes using ASP.NET Web API's self host with scriptcs easy as cake, much easier than [this] (https://github.com/scriptcs/scriptcs-samples/tree/master/webapihost) :)

## Highlights:

* Creates a pre-configured self host with the default route already added.
* Configures to allow resolving script controllers.
* Automatically imports common web api namespaces for you.

## Getting started with Web API using the pack

Disclaimer: Ultimately (soon) you will be able to install this via nuget and not have to clone / build / copy

* Clone this repo (if you are seeing this before my [PR] (https://github.com/scriptcs/scriptcs/pull/177) was accepted, then be sure to first clone my [fork] (https://github.com/glennblock/scriptcs/tree/147) of scriptcs and use it.
* Build the solution (make sure you enable package restore).
* Create a new folder for your script i.e. c:\hellowebapi and change to it.
* Install the Web Api Self Host nuget package ```scriptcs -install Microsoft.AspNet.WebApi.SelfHost```
* Copy ScriptCs.WebApi.Pack.dll, and ScriptCs.Contracts.dll from the script pack bin folder to your local bin.
* Create a start.csx and paste the code below

```csharp
public class TestController : System.Web.Http.ApiController {
  public string Get() {
    return "Hello world!";
  }
}

var webApi = Require<WebApi>();
var server = webApi.CreateServer("http://localhost:8080");
server.OpenAsync().Wait();
 
Console.WriteLine("Listening...");
Console.ReadKey();
```
* Running as admin type ```scriptcs start.csx``` to launch the app.
* Open a browser to "http://localhost:8080/test";
* That's it, your API is up!

## Customizing
You can customize the host by modifying the configuration object. Or if you would like to pass your own you can use the CreateServer overload.
If you pass your own, the ControllerHttpResolver will be replaced with a script friendly one.

## What's next
* Create a nuget package.
