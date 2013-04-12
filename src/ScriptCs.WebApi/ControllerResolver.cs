using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Http.Dispatcher;

namespace ScriptCs.WebApi
{
    internal class ControllerResolver : DefaultHttpControllerTypeResolver
    {
        private ICollection<Type> _controllerTypes;

        internal ControllerResolver(ICollection<Type> controllerTypes)
        {
            Contract.Requires(AllAssignableToIHttpController(controllerTypes));

            _controllerTypes = controllerTypes;
        }

        public override ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            Contract.Invariant(_controllerTypes != null);

            return _controllerTypes;
        }

        internal static IEnumerable<Type> WhereControllerType(IEnumerable<Type> types)
        {
            Contract.Requires(types != null);

            return types.Where(x => typeof(System.Web.Http.Controllers.IHttpController).IsAssignableFrom(x));
        }

        internal static bool AllAssignableToIHttpController(IEnumerable<Type> types)
        {
            Contract.Requires(types != null);

            return types.All(x => typeof(System.Web.Http.Controllers.IHttpController).IsAssignableFrom(x));
        }
    }
}
