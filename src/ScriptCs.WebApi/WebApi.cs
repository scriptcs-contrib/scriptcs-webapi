using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using ScriptCs.Contracts;

namespace ScriptCs.WebApi
{
    public class WebApi : IScriptPackContext
    {
        public HttpSelfHostServer CreateServer(HttpSelfHostConfiguration config, ICollection<Type> controllerTypes)
        {
#if DEBUG
            Console.WriteLine("Using the following types to find controllers:");
            foreach (Type type in controllerTypes)
            {
                Console.WriteLine(" - " + type.ToString());
            }
#endif

            config.Services.Replace(typeof(IHttpControllerTypeResolver), new ControllerResolver(controllerTypes));

            config.Routes.MapHttpRoute(name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            return new HttpSelfHostServer(config);
        }

        public HttpSelfHostServer CreateServer(HttpSelfHostConfiguration config, Assembly caller = null)
        {
            if (caller == null)
            {
                caller = Assembly.GetCallingAssembly();
            }

            return CreateServer(config, caller.GetTypes());
        }

        public HttpSelfHostServer CreateServer(string baseAddress)
        {
            var caller = Assembly.GetCallingAssembly();
            return CreateServer(new HttpSelfHostConfiguration(baseAddress), caller.GetTypes());
        }
    }
}