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
    /// <summary>
    /// Creates a web api either as a self hosetd server of a OWIN middleware component
    /// </summary>
    public class WebApi : IScriptPackContext
    {
        /// <summary>
        /// Create a new self hosted WebApi server
        /// </summary>
        /// <param name="config">The configuration to for the server</param>
        /// <param name="controllerTypes">The controller types to load</param>
        /// <returns>The self host server instance</returns>
        public HttpSelfHostServer CreateServer(HttpSelfHostConfiguration config, ICollection<Type> controllerTypes)
        {
            ApplyDefaultConfiguration((HttpConfiguration)config, controllerTypes);

            return new HttpSelfHostServer(config);
        }

        /// <summary>
        /// Create a new self hosted WebApi server
        /// </summary>
        /// <param name="config">The configuration to for the server</param>
        /// <param name="assemblies">The assmemblies to search for controllers</param>
        /// <returns>The seld host server instance</returns>
        public HttpSelfHostServer CreateServer(HttpSelfHostConfiguration config, params Assembly[] assemblies)
        {
            return CreateServer(config, GetControllerTypes(assemblies));
        }

        /// <summary>
        /// Create a new self hosted WebApi server
        /// </summary>
        /// <param name="baseAddress">The base address for the service</param>
        /// <returns>The self host server instance</returns>
        public HttpSelfHostServer CreateServer(string baseAddress)
        {
            return CreateServer(new HttpSelfHostConfiguration(baseAddress),  GetControllerTypes());
        }

        /// <summary>
        /// Creates a WebApi server with defined list of controllers
        /// </summary>
        /// <param name="config">The configuration to create the WebApi instance from</param>
        /// <param name="controllerTypes">The controller types to load</param>
        public HttpConfiguration Create(HttpConfiguration config, ICollection<Type> controllerTypes)
        {
            ApplyDefaultConfiguration(config, controllerTypes);

            return config;
        }

        /// <summary>
        /// Creates a WebApi server with all available controllers from defined assemblies
        /// </summary>
        /// <param name="config">The configuration to create the WebApi instance from</param>
        /// <param name="assemblies">The assmemblies to search for controllers</param>
        public HttpConfiguration Create(HttpConfiguration config, params Assembly[] assemblies)
        {
            return Create(config, GetControllerTypes(assemblies));
        }

        /// <summary>
        /// Creates a WebApi server with all available controllers
        /// </summary>
        public HttpConfiguration Create()
        {
            return Create(new HttpConfiguration(), GetControllerTypes());
        }

        private List<Type> GetControllerTypes()
        {
            IEnumerable<Type> types = GetLoadedTypes();
            List<Type> controllerTypes = ControllerResolver.WhereControllerType(types).ToList();
            return controllerTypes;
        }

        private static readonly string[] IgnoredAssemblyPrefixes = new[]
            {
                "Autofac,",
                "Autofac.",
                "Common.Logging",
                "log4net,",
                "Nancy,",
                "Nancy.",
                "NuGet.",
                "PowerArgs,",
                "Roslyn.",
                "scriptcs,",
                "ScriptCs.",
                "ServiceStack.",
            };

        public IEnumerable<Type> GetLoadedTypes()
        {
            var types = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !IgnoredAssemblyPrefixes.Any(p => a.GetName().Name.StartsWith(p))))
            {
                try
                {
                    types.AddRange(assembly.GetTypes());
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Console.WriteLine("Count not load types from {0}", assembly.FullName);
                    foreach (var load in ex.LoaderExceptions)
                    {
                        Console.WriteLine("Exception {0}", load);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Count not load types from {0} - {0}", assembly.FullName, ex);
                }
                
            }
            return types;
        }

        private static void ApplyDefaultConfiguration(HttpConfiguration config,
                                                      ICollection<Type> controllerTypes)
        {
            Contract.Requires(controllerTypes != null);
            Contract.Requires(ControllerResolver.AllAssignableToIHttpController(controllerTypes));

            config.Services.Replace(typeof (IHttpControllerTypeResolver), new ControllerResolver(controllerTypes));

            config.Routes.MapHttpRoute(name: "DefaultApi",
                                       routeTemplate: "api/{controller}/{id}",
                                       defaults: new {id = RouteParameter.Optional}
                );
        }

        private static List<Type> GetControllerTypes(Assembly[] assemblies)
        {
            IEnumerable<Assembly> controllerAssemblies =
                new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies()).Union(assemblies);
            Type[] types = controllerAssemblies.SelectMany(a => a.GetTypes()).ToArray();
            List<Type> controllerTypes = ControllerResolver.WhereControllerType(types).ToList();
            return controllerTypes;
        }
    }
}