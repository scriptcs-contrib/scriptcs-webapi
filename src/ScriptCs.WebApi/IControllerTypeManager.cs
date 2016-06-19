using System;
using System.Collections.Generic;
using System.Reflection;

namespace ScriptCs.WebApi
{
    public interface IControllerTypeManager
    {
        IEnumerable<Type> GetControllerTypes(Assembly[] assemblies);
        IEnumerable<Type> GetControllerTypes();
    }
}