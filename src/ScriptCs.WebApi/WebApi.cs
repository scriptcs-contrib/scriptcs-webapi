using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using Common.Logging;
using ScriptCs.Contracts;

namespace ScriptCs.WebApi
{
    /// <summary>
    /// Creates a web api either as a self hosetd server of a OWIN middleware component
    /// </summary>
    public class WebApi : IScriptPackContext
    {
        private readonly ILog _logger;
        private readonly IControllerTypeManager _typeManager;

        public WebApi(ILog logger, IControllerTypeManager typeManager)
        {
            _logger = logger;
            _typeManager = typeManager;
        }

        /// <summary>
        /// Create a new self hosted WebApi server
        /// </summary>
        /// <param name="config">The configuration to for the server</param>
        /// <param name="controllerTypes">The controller types to load</param>
        /// <returns>The self host server instance</returns>
        public HttpSelfHostServer CreateServer(HttpSelfHostConfiguration config, IEnumerable<Type> controllerTypes)
        {
            ApplyDefaultConfiguration(config, controllerTypes);

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
            return CreateServer(config, _typeManager.GetControllerTypes(assemblies));
        }

        /// <summary>
        /// Create a new self hosted WebApi server
        /// </summary>
        /// <param name="baseAddress">The base address for the service</param>
        /// <returns>The self host server instance</returns>
        public HttpSelfHostServer CreateServer(string baseAddress)
        {
            return CreateServer(new HttpSelfHostConfiguration(baseAddress),  _typeManager.GetControllerTypes());
        }

        /// <summary>
        /// Creates a WebApi server with defined list of controllers
        /// </summary>
        /// <param name="config">The configuration to create the WebApi instance from</param>
        /// <param name="controllerTypes">The controller types to load</param>
        public HttpConfiguration Configure(HttpConfiguration config, IEnumerable<Type> controllerTypes)
        {
            ApplyDefaultConfiguration(config, controllerTypes);

            return config;
        }

        /// <summary>
        /// Creates a WebApi server with all available controllers from defined assemblies
        /// </summary>
        /// <param name="config">The configuration to create the WebApi instance from</param>
        /// <param name="assemblies">The assmemblies to search for controllers</param>
        public HttpConfiguration Configure(HttpConfiguration config, params Assembly[] assemblies)
        {
            return Configure(config, _typeManager.GetControllerTypes(assemblies));
        }

        /// <summary>
        /// Creates a WebApi server with all available controllers
        /// </summary>
        public HttpConfiguration Configure()
        {
            return Configure(new HttpConfiguration(), _typeManager.GetControllerTypes());
        }

        private static void ApplyDefaultConfiguration(HttpConfiguration config,
                                                      IEnumerable<Type> controllerTypes)
        {
            Contract.Requires(controllerTypes != null);
            Contract.Requires(ControllerResolver.AllAssignableToIHttpController(controllerTypes));

            config.Services.Replace(typeof (IHttpControllerTypeResolver), new ControllerResolver(controllerTypes.ToList()));

            config.Routes.MapHttpRoute(name: "DefaultApi",
                                       routeTemplate: "api/{controller}/{id}",
                                       defaults: new {id = RouteParameter.Optional}
                );
        }
    }
}