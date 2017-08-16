using System;
using System.Threading.Tasks;
using System.Linq;

using WebSharpJs.NodeJS;
using WebSharpJs.Electron;
using WebSharpJs.DOM;
using WebSharpJs.Script;

//namespace ClapsRenderer
//{
    public class Startup
    {

        static WebSharpJs.NodeJS.Console console;

        HtmlElement area;
        HtmlElement output;
        HtmlElement spaces;
        HtmlElement emojiPicker;
        HtmlElement length;
        HtmlElement copy;

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
                var document = await HtmlPage.GetDocument();
                area = await document.GetElementById("input");
                output = await document.GetElementById("output");
                spaces = await document.GetElementById("spaced");
                emojiPicker = await document.GetElementById("emoji");
                length = await document.GetElementById("length");
                copy = await document.GetElementById("copy");

                await area.AttachEvent(HtmlEventNames.Input, updateOutput);
                await spaces.AttachEvent(HtmlEventNames.Change, updateOutput);
                await emojiPicker.AttachEvent(HtmlEventNames.Change, updateOutput);
                await copy.AttachEvent(HtmlEventNames.Click, copyClicked);

                await console.Log($"Hello:  {input}");
            }
            catch (Exception exc) { await console.Log($"extension exception:  {exc.Message}"); }

            return null;


        }

        async void updateOutput (object sender, EventArgs args)
        {
            
            var words = (await area.GetProperty<string>("value")).Split(' ');
            var emoji = await emojiPicker.GetProperty<string>("value");
            var joiner = (await spaces.GetProperty<bool>("checked")) ? " " + emoji + " " : emoji;
            var result = String.Join(joiner, words);

            await output.SetProperty("value", result);

            if (result.Length == 0)
                await length.SetProperty("innerHTML", "");    
            else
                await length.SetProperty("innerHTML", $"({result.Length}/140)");

            if (result.Length > 140)
            {
                await addClass(length, "text-danger");
            }
            else 
            {
                await removeClass(length, "text-danger");
            }
        }
        async void copyClicked (object sender, EventArgs args)
        {
            var clipboard = await Clipboard.Instance();
            await clipboard.Write(new ClipboardData() { Text = await output.GetProperty<string>("value") });
        }
        
        static readonly char[] rnothtmlwhite = new char[] {' ','\r','\n','\t','\f' };
        bool IsHasClass(string elementClass, string className)
        {
            if (string.IsNullOrEmpty(elementClass) || string.IsNullOrEmpty(className))
                return false;
            return elementClass.Split(rnothtmlwhite, StringSplitOptions.RemoveEmptyEntries).Contains(className);
        }

        async Task<string> addClass(HtmlElement element, string klass, string elementClass = null)
        {
            if (string.IsNullOrEmpty(elementClass))
            {
                elementClass = await length.GetCssClass();
            }

            if (!IsHasClass(elementClass, klass))
            {
                elementClass += $" {klass} ";
                await element.SetCssClass(elementClass);
            }
            return elementClass;
        }
        async Task<bool> removeClass(HtmlElement element, string klass, string elementClass = null)
        {
            if (string.IsNullOrEmpty(elementClass))
            {
                elementClass = await length.GetCssClass();
            }

            if (IsHasClass(elementClass, klass))
            {
                var classList = elementClass.Split(rnothtmlwhite, StringSplitOptions.RemoveEmptyEntries).ToList();
                classList.Remove(klass);
                await element.SetCssClass(string.Join(" ",classList));
                return true;
            }
            return false;
        }
    }
//}
