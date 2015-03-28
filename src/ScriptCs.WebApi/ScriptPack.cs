﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ScriptCs;
using ScriptCs.Contracts;

namespace ScriptCs.WebApi
{
    public class ScriptPack : IScriptPack
    {
        private readonly IControllerTypeManager _typeManager;

        [ImportingConstructor]
        public ScriptPack(IControllerTypeManager typeManager)
        {
            _typeManager = typeManager;
        }

        IScriptPackContext IScriptPack.GetContext()
        {
            return new WebApi(_typeManager);
        }

        void IScriptPack.Initialize(IScriptPackSession session)
        {
            session.AddReference("System.Net.Http");
            var namespaces = new[]
                {
                    "System.Web.Http",
                    "System.Web.Http.Routing",
                    "System.Net.Http",
                    "System.Net.Http.Headers",
                    "Owin"
                }.ToList();

            namespaces.ForEach(session.ImportNamespace);
        }

        void IScriptPack.Terminate()
        {
        }
    }
}
