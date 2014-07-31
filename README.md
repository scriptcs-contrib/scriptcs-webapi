scriptcs-webapi
===============

# Web API Script Pack

## What is it?
Makes using ASP.NET Web API's self host with scriptcs easy as cake, much easier than [this] (https://github.com/scriptcs/scriptcs-samples/tree/master/webapihost) :)

## Highlights:

* Creates a pre-configured OWIN self host with the default routes already added.
* Supports configuring the OWIN pipeline
* Write your controllers as scripts
* Automatically imports common web api namespaces for you.

## Getting started with Web API using the pack

* Create a new folder for your script i.e. c:\hellowebapi and change to it.
* Install the Web API script pack ```scriptcs -install -pre ScriptCs.WebApi2```
* Create a start.csx and paste the code below

```csharp
using System.Dynamic;

public class TestController : ApiController
{
	public dynamic Get() {
		dynamic obj = new ExpandoObject();
		obj.message = "Hello from Web Api";
		return obj;
	}
}

var webapi = Require<WebApi>();

var server = webapi.
	Configure(typeof(TestController)).
	UseJsonOnly().
	Start("http://localhost:8080");

Console.WriteLine("Listening...");
Console.ReadLine();
server.Dispose();
```
* Running as admin type ```scriptcs start.csx -modules mono``` on Windows or ```scriptcs start.csx``` on Mac/Linux to launch the app.
* Open a browser to "http://localhost:8080/api/test";
* That's it, your API is up!

## Customizing
You can configure the OWIN host by passing in an `Action<IBuilder>` to the `Configure` method
```csharp
var server = webapi.
	Configure(
		typeof(TestController),
		builder=> {
		  //builder.Use<MyMiddleware>();
		}
	).
	Start("http://localhost:8080");
```
