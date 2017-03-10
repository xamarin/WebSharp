using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSharpJs.Browser
{
    public sealed class HtmlPage 
    {
        HtmlDocument htmlDocument;
        public async Task<HtmlDocument> GetDocument()
        {
            if (htmlDocument == null)
                return await HtmlDocument.Instance();
            else
                return htmlDocument;
        }
    }
}
