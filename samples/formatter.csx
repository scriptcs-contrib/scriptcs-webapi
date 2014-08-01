using System.Dynamic;
using System.Web.Http;
using System.Web.Http.Routing;

public class TestController : ApiController
{
	public dynamic Get() {
		dynamic obj = new ExpandoObject();
		obj.message = "Hello from Web Api";
		return obj;
	}
}

var webapi = Require<WebApi>();

var formatter = webapi.NewFormatter().
	SupportMediaType("application/vnd.foo+json").
	MapUriExtension(".foo", "application/vnd.foo+json").
	WriteToStream(async (args) => {
		var writer = new StreamWriter(args.Stream);
		await writer.WriteLineAsync("{\"foo\":\"foo\"}");
		await writer.FlushAsync();
	}).
	Build();

var config = new HttpConfiguration();

webapi.
	UseJsonOnly().
	Configure(config, typeof(TestController));

config.Formatters.Insert(0, formatter);	

config.Routes.Clear();
config.Routes.MapHttpRoute(name: "Extension",
	routeTemplate: "api/{controller}.{extension}/{id}",
	defaults: new {id = RouteParameter.Optional}
);

var server = webapi.Start("http://localhost:8080");

Console.WriteLine("Listening...");
Console.ReadLine();
server.Dispose();


