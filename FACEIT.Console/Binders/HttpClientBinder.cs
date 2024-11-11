using System;
using System.Collections.Generic;
using System.CommandLine.Binding;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Console.Binders
{
    internal class HttpClientBinder : BinderBase<HttpClient>
    {
        protected override HttpClient GetBoundValue(BindingContext bindingContext)
        {
            return new HttpClient();
        }
    }
}
