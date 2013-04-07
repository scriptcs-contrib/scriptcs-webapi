using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace ScriptCs.WebApi
{
    public class ControllerResolver : DefaultHttpControllerTypeResolver
    {
        private Assembly _scriptAssembly;

        public ControllerResolver(Assembly scriptAssembly)
        {
            _scriptAssembly = scriptAssembly;
        }

        public override ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            var types = _scriptAssembly.GetTypes().ToList();
            var controllers = types.Where(x => typeof(System.Web.Http.Controllers.IHttpController).IsAssignableFrom(x)).ToList();
            return controllers;
        }
    }
}
