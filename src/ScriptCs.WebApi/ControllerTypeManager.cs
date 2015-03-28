using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ScriptCs.WebApi.Logging;

namespace ScriptCs.WebApi
{
    [Export(typeof(IControllerTypeManager))]
    public class ControllerTypeManager : IControllerTypeManager
    {
        private static readonly ILog _logger = LogProvider.GetCurrentClassLogger();
        private const string RoslynAssemblyNameCharacter = "ℛ";

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
                    _logger.Debug("Scriptcs.WebApi - Loaded assembly " + assembly);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    _logger.Warn(string.Format("ScriptCs.WebApi - Count not load types from {0}", assembly.FullName));
                    foreach (var load in ex.LoaderExceptions)
                    {
                        _logger.Warn(string.Format("ScriptCs.WebApi - Exception {0}", load));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Warn(string.Format("ScriptCs.WebApi - Count not load types from {0}: {1}", assembly.FullName, ex.Message));
                }

            }
            return types;
        }

        public IEnumerable<Type> GetControllerTypes()
        {
            var types = GetLoadedTypes();
            var tempControllerTypes = ControllerResolver.WhereControllerType(types).ToList();
            var controllerTypes = tempControllerTypes.Where(t => t.Assembly.FullName.Substring(0, 1) != RoslynAssemblyNameCharacter).ToList();
            var roslynTypes = tempControllerTypes.Where(t => t.Assembly.FullName.StartsWith(RoslynAssemblyNameCharacter));
            controllerTypes.Add(roslynTypes.Last());
            foreach (var controller in controllerTypes)
            {
                _logger.Debug(string.Format("ScriptCs.WebApi - Found controller: {0}", controller.FullName));
            }
            return controllerTypes;
        }

        public IEnumerable<Type> GetControllerTypes(Assembly[] assemblies)
        {
            IEnumerable<Assembly> controllerAssemblies =
                new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies()).Union(assemblies);
            Type[] types = controllerAssemblies.SelectMany(a => a.GetTypes()).ToArray();
            List<Type> controllerTypes = ControllerResolver.WhereControllerType(types).ToList();
            return controllerTypes;
        }


    }
}
