using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dispatcher;

namespace ScriptCs.WebApi
{
    public class ControllerResolver : DefaultHttpControllerTypeResolver
    {
        private ICollection<Type> _controllerTypes;

        public ControllerResolver(ICollection<Type> controllerTypes)
        {
            _controllerTypes = controllerTypes.Where(x => typeof(System.Web.Http.Controllers.IHttpController).IsAssignableFrom(x)).ToList();
        }

        public override ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            return _controllerTypes;
        }
    }
}
