using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using ScriptCs.Contracts;

namespace ScriptCs.WebApi
{
    public class WebApi : IScriptPackContext
    {

        public HttpSelfHostServer CreateServer(HttpSelfHostConfiguration config)
        {
            var caller = System.Reflection.Assembly.GetCallingAssembly();
            config.Services.Replace(typeof(IHttpControllerTypeResolver), new ControllerResolver(caller));

            config.Routes.MapHttpRoute(name: "DefaultApi",
                                       routeTemplate: "api/{controller}/{id}",
                                       defaults: new { id = RouteParameter.Optional }
                );
            return new HttpSelfHostServer(config);
        }

        public HttpSelfHostServer CreateServer(string baseAddress)
        {
            return CreateServer(new HttpSelfHostConfiguration(baseAddress));
        }
    }
}