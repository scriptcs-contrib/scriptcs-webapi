using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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
            Contract.Requires(controllerTypes != null);
            Contract.Requires(ControllerResolver.AllAssignableToIHttpController(controllerTypes));

            config.Services.Replace(typeof(IHttpControllerTypeResolver), new ControllerResolver(controllerTypes));

            config.Routes.MapHttpRoute(name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            return new HttpSelfHostServer(config);
        }

        public HttpSelfHostServer CreateServer(HttpSelfHostConfiguration config, params Assembly[] assemblies)
        {
            var controllerAssemblies = new List<Assembly> {Assembly.GetCallingAssembly()}.Union(assemblies);
            var types = controllerAssemblies.SelectMany(a => a.GetTypes()).ToArray();
            var controllerTypes = ControllerResolver.WhereControllerType(types).ToList();
            return CreateServer(config, controllerTypes);
        }

        public HttpSelfHostServer CreateServer(string baseAddress)
        {
            var types = Assembly.GetCallingAssembly().GetTypes();
            var controllerTypes = ControllerResolver.WhereControllerType(types).ToList();
            return CreateServer(new HttpSelfHostConfiguration(baseAddress), controllerTypes);
        }
    }
}