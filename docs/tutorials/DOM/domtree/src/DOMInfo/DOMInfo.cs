using System;
using System.Threading.Tasks;

using WebSharpJs.NodeJS;
using WebSharpJs.DOM;

//namespace DOMInfo
//{
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

        string elementTree = string.Empty;

        /// <summary>
        /// Default entry into managed code.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<object> Invoke(object input)
        {
            if (console == null)
                console = await WebSharpJs.NodeJS.Console.Instance();

            try
            {
                var page = new HtmlPage();
                var document = await page.GetDocument();
                // Get a reference to the top-level <html> element.
                var element = await document.GetDocumentElement();
                // Process the starting element reference
                await ProcessElement(element, 0);
                await console.Log(elementTree);

                await console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }

        private async Task ProcessElement(HtmlElement element, int indent)
        {
            // Ignore comments.
            if (await element.GetTagName() == "!") return;
            
            // Indent the element to help show different levels of nesting.
            elementTree += new String(' ', indent * 4);

            // Display the tag name.
            elementTree += "<" + await element.GetTagName();
            
            // Only show the id attribute if it's set.
            if (await element.GetId() != "") elementTree += " id=\"" + await element.GetId() + "\"";
            elementTree += ">\n";

            // Process all the elements nested inside the current element.
            foreach (var childElement in await element.GetChildren())
            {
                await ProcessElement(childElement, indent + 1);
            }
        }
    }
//}
