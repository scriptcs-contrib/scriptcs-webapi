using System.ComponentModel.Composition;
using System.Linq;
using ScriptCs.Contracts;

namespace ScriptCs.WebApi
{
    public class ScriptPack : IScriptPack
    {
        private readonly ILog _logger;
        private readonly IControllerTypeManager _typeManager;

        [ImportingConstructor]
        public ScriptPack(ScriptCs.Contracts.ILogProvider loggerProvider, IControllerTypeManager typeManager)
        {
            _logger = loggerProvider.ForCurrentType();
            _typeManager = typeManager;
        }

        IScriptPackContext IScriptPack.GetContext()
        {
            return new WebApi(_logger, _typeManager);
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
