using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using ScriptCs.Contracts;

namespace ScriptCs.WebApi
{
    public class WebApi : IScriptPackContext
    {

        public HttpSelfHostServer CreateServer(HttpSelfHostConfiguration config, Assembly caller = null)
        {
            if (caller == null)
            {
                caller = Assembly.GetCallingAssembly();
            }

            config.Services.Replace(typeof(IHttpControllerTypeResolver), new ControllerResolver(caller));

            config.Routes.MapHttpRoute(name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            return new HttpSelfHostServer(config);
        }

        public HttpSelfHostServer CreateServer(string baseAddress)
        {
            var caller = Assembly.GetCallingAssembly();
            return CreateServer(new HttpSelfHostConfiguration(baseAddress), caller);
        }
    }
}