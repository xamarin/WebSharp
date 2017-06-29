using System;
using System.Threading.Tasks;
using System.Linq;

using WebSharpJs.NodeJS;
using WebSharpJs.DOM;
using WebSharpJs.Script;

//namespace ElementTinkerer
//{
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

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
                // Get a reference to the HTML DOM document.
                var document = await HtmlPage.GetDocument();

                // Get a reference to the body element.
                var body = await document.GetElementById("docBody");
                // Create a <main> type element
                var mainElement = await document.CreateElement("main");
                // Append the newly created element to the page DOM
                await body.AppendChild(mainElement);
                
                // Get a collection of all the <main> type element in the page and using
                // Linq get the first of the collection.
                var content = (await document.GetElementsByTagName("main")).First();

                // Get a collection of all the <link> elements
                var links = await document.QuerySelectorAll("link[rel=\"import\"]");

                // Loop through all of the <link> elements found
                foreach (var link in links)
                {
                    // Access the contents of the import document by examining the 
                    // import property of the corresponding <link> element
                    var template = await link.GetProperty<HtmlElement>("import").ContinueWith(
                            async (t) =>
                            {
                                // For all imported contents obtain a reference to the <template>
                                return await t.Result?.QuerySelector(".task-template");
                            }
                        ).Result;
                    // Create a clone of the template’s content using the importNode property and
                    // passing in the content of the template
                    var clone = await document.Invoke<HtmlElement>("importNode", 
                                await template.GetProperty<HtmlElement>("content"), true);

                    // Append the newly created cloned template to the content element.
                    await content.AppendChild(clone);
                }
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }
    }
//}
