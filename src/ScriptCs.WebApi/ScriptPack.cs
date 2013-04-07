using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptCs;
using ScriptCs.Contracts;

namespace ScriptCs.WebApi
{
    public class ScriptPack : IScriptPack
    {
        IScriptPackContext IScriptPack.GetContext()
        {
            return new WebApi();
        }

        void IScriptPack.Initialize(IScriptPackSession session)
        {
            var namespaces = new[]
                {
                    "System.Web.Http",
                    "System.Web.Http.SelfHost",
                    "System.Web.Http.Dispatcher"
                }.ToList();

            namespaces.ForEach(session.ImportNamespace);
        }

        void IScriptPack.Terminate()
        {
        }
    }
}
