scriptcs-webapi
===============

# Web API Script Pack

## What is it?
Makes using ASP.NET Web API's self host with scriptcs easy as cake, much easier than [this] (https://github.com/scriptcs/scriptcs-samples/tree/master/webapihost) :)

## Highlights:

* Creates a pre-configured self host with the default route already added.
* Configures to allow resolving script controllers.
* Automatically imports common web api namespaces for you.
* Works as self hosted server or in OWIN

## Getting started with Web API using the pack

Disclaimer: Ultimately (soon) you will be able to install this via nuget and not have to clone / build / copy

* Create a new folder for your script i.e. c:\hellowebapi and change to it.
* Install the Web API script pack ```scriptcs -install -pre ScriptCs.WebApi```
* Create a start.csx and paste the code below

```csharp
public class TestController : ApiController {
  public string Get() {
    return "Hello world!";
  }
}

var webApi = Require<WebApi>();
var server = webApi.CreateServer("http://localhost:8080");
server.OpenAsync().Wait();
 
Console.WriteLine("Listening...");
Console.ReadLine();
server.CloseAsync().Wait();
```
* Running as admin type ```scriptcs start.csx``` to launch the app.
* Open a browser to "http://localhost:8080/api/test";
* That's it, your API is up!

Alternatively you can host the script pack in OWIN e.g.

```csharp
public class TestController : ApiController {
  public string Get() {
    return "Hello world!";
  }
}

Require<OwinSelfHost>();

var webApi = Require<WebApi>();
var config = webApi.Create();

using ( OwinSelfHost.CreateServer("http://localhost:8080", app => app.UseWebApi(config)) ) {
	Console.WriteLine("Listening...");
	Console.ReadLine();
}
```


## Customizing
You can customize the host by modifying the configuration object.
Or if you would like to pass your own you can use the `CreateServer` overload.
Additional `CreateServer` overloads allow you to explicitly specify assemblies or `IHttpController` types you want to expose in your api:

```csharp
// Use a custom configuration and specify controller types.
var config = new HttpSelfHostConfiguration("http://localhost:8080");
var controllers = new List<Type> { typeof(TestController) };
var server = webApi.CreateServer(config, controllers)
```

## What's next
TBD
