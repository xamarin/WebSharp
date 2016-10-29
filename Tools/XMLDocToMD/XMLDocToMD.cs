using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace GithubWikiDoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var xml = File.ReadAllText(args[0]);
            var doc = XDocument.Parse(xml);

            if (args.Length > 1)
            {


                Console.WriteLine($"# {args[1]} Class Reference\n## Description\n");

                var classDescription = doc.Root.Elements("members").Elements("member").Where(
                    member => member.Attribute("name").Value == "T:" + args[1]
                );

                var description = classDescription.Aggregate("", (current, x) => current + x.ToMarkDown());

                // Remove class name from parsed mark down text
                description = description.Replace("# T:" + args[1] + "\n\n", "");

                if (description.Length > 0)
                    Console.WriteLine(description);

                var mdTable = "\n## Constructors\n| Name | Description |\n| --- | ----------- |\n";

                var tableElements = doc.Root.Elements("members").Elements("member").Where(
                        member => member.Attribute("name").Value.Substring(2).StartsWith(args[1]+".#ctor") && member.Attribute("name").Value.Substring(0, 1) == "M"
                    );

                var mdList = tableElements.Aggregate("", (current, x) => current + x.ToMarkDownList());

                // Fix up name
                mdList = mdList.Replace("| [M:" + args[1] + ".", "| [");

                if (mdList.Length > 0)
                    Console.WriteLine(mdTable + mdList);

                mdTable = "\n## Properties\n| Name | Description |\n| --- | ----------- |\n";

                tableElements = doc.Root.Elements("members").Elements("member").Where(
                        member => member.Attribute("name").Value.Substring(2).StartsWith(args[1]) && member.Attribute("name").Value.Substring(0,1) == "P"
                    );

                mdList = tableElements.Aggregate("", (current, x) => current + x.ToMarkDownList());

                // Fix up name
                mdList = mdList.Replace("| [P:" + args[1] + ".", "| [");

                if (mdList.Length > 0)
                    Console.WriteLine(mdTable + mdList);

                mdTable = "\n## Methods\n| Name | Description |\n| --- | ----------- |\n";

                tableElements = doc.Root.Elements("members").Elements("member").Where(
                        member => !member.Attribute("name").Value.Substring(2).StartsWith(args[1] + ".#ctor") && member.Attribute("name").Value.Substring(2).StartsWith(args[1]) && member.Attribute("name").Value.Substring(0, 1) == "M"
                    );

                mdList = tableElements.Aggregate("", (current, x) => current + x.ToMarkDownList());

                // Fix up name
                mdList = mdList.Replace("| [M:" + args[1] + ".", "| [");

                if (mdList.Length > 0)
                    Console.WriteLine(mdTable + mdList);

                mdTable = "\n## Events\n| Event | Description |\n| --- | ----------- |\n";

                tableElements = doc.Root.Elements("members").Elements("member").Where(
                        member => member.Attribute("name").Value.Substring(2).StartsWith(args[1]) && member.Attribute("name").Value.Substring(0, 1) == "E"
                    );

                mdList = tableElements.Aggregate("", (current, x) => current + x.ToMarkDownList());

                // Fix up name
                mdList = mdList.Replace("| [E:" + args[1] + ".", "| [");

                if (mdList.Length > 0)
                    Console.WriteLine(mdTable + mdList);

                var classMembers = doc.Root.Elements("members").Elements("member").Where(
                        member => member.Attribute("name").Value.Substring(2).StartsWith(args[1])
                    );

                var md = classMembers.Aggregate("", (current, x) => current + x.ToMarkDown());
                Console.WriteLine(md);
            }
            else
            {
                var md = doc.Root.ToMarkDown();
                Console.WriteLine(md);
            }
        }
    }

    static class XmlToMarkdown
    {
        internal static string ToMarkDown(this XNode e)
        {
            var properties = new List<string> { "| Name | Description |\n| --- | ----------- |" };

            var templates = new Dictionary<string, string>
                {
                    {"doc", "## {0} ##\n\n{1}\n\n"},
                    {"type", "# {0}\n\n{1}\n\n---\n"},
                    {"field", "##### {0}\n\n{1}\n\n---\n"},
                    {"property", "##### {0}\n\n{1}\n\n---\n"},
                    {"globalProperty", "##### {0}\n\n{1}\n\n---\n"},
                    {"method", "##### {0}\n\n{1}\n\n---\n"},
                    {"event", "##### {0}\n\n{1}\n\n---\n"},
                    {"summary", "{0}\n\n"},
                    {"remarks", "\n\n>{0}\n\n"},
                    {"example", "_C# code_\n\n```c#\n{0}\n```\n\n"},
                    {"seePage", "[[{1}|{0}]]"},
                    {"seeAnchor", "[{1}]({0})"},
                    {"param", "|Name | Description |\n|-----|------|\n|{0}: |{1}|\n" },
                    {"exception", "[[{0}|{0}]]: {1}\n\n" },
                    {"returns", "Returns: {0}\n\n"},
                    {"externalLink", "[{1}]({0})"},
                    {"code", "`{0}`"},
                    {"embed", "`<embed></embed>`"},
                    {"bold", "**{0}**"},
                    {"italic", "__{0}__"},
                    {"body", "_Java Script_\n\n``` js\n<body>\n   {0}\n<\body>\n```\n\n"},
                    {"none", ""}
                };

            var d = new Func<string, XElement, string[]>((att, node) => new[]
                {
                    node.Attribute(att).Value,
                    node.Nodes().ToMarkDown()
                });

            var methods = new Dictionary<string, Func<XElement, IEnumerable<string>>>
                {
                    {"doc", x=> new[]{
                        x.Element("assembly").Element("name").Value,
                        x.Element("members").Elements("member").ToMarkDown()
                    }},
                    {"type", x=>d("name", x)},
                    {"field", x=> d("name", x)},
                    {"property", x=> d("name", x)},
                    {"method",x=>d("name", x)},
                    {"event", x=>d("name", x)},
                    {"summary", x=> new[]{ x.Nodes().ToMarkDown() }},
                    {"remarks", x => new[]{x.Nodes().ToMarkDown()}},
                    {"example", x => new[]{x.Value.ToCodeBlock()}},
                    {"seePage", x=> d("cref", x) },
                    {"seeAnchor", x=> { var xx = d("cref", x); xx[0] = xx[0].ToLower(); return xx; }},
                    {"param", x => d("name", x) },
                    {"exception", x => d("cref", x) },
                    {"returns", x => new[]{x.Nodes().ToMarkDown()}},
                    {"externalLink", x => d("href", x) },
                    {"code", x => new[]{x.Nodes().ToMarkDown()}},
                    {"embed", x => new[]{x.Nodes().ToMarkDown()}},
                    {"bold", x => new[]{x.Nodes().ToMarkDown()}},
                    {"italic", x => new[]{x.Nodes().ToMarkDown()}},
                    {"body", x => new[]{x.Value.ToCodeBlock()}},
                    {"none", x => new string[0]}
                };

            string name;
            if (e.NodeType == XmlNodeType.Element)
            {
                var el = (XElement)e;
                name = el.Name.LocalName;

                if (name == "member")
                {
                    switch (el.Attribute("name").Value[0])
                    {
                        case 'F': name = "field"; break;
                        case 'P': name = "property"; break;
                        case 'T': name = "type"; break;
                        case 'E': name = "event"; break;
                        case 'M': name = "method"; break;
                        default: name = "none"; break;
                    }
                }
                if (name == "see")
                {
                    var anchor = el.Attribute("cref").Value.StartsWith("!:#");
                    name = anchor ? "seeAnchor" : "seePage";
                }
                if (name == "code")
                {
                    name = "code";
                }
                if (name == "embed")
                {
                    name = "embed";
                }
                if (name == "a")
                {
                    name = "externalLink";
                }
                if (name == "strong")
                {
                    name = "bold";
                }
                if (name == "i")
                {
                    name = "italic";
                }
                if (name == "body")
                {
                    name = "body";
                }
                if (name == "typeparam")
                {
                    name = "param";
                }
                var vals = methods[name](el).ToArray();
                string str = "";
                switch (vals.Length)
                {
                    case 1: str = string.Format(templates[name], vals[0]); break;
                    case 2: str = string.Format(templates[name], vals[0], vals[1]); break;
                    case 3: str = string.Format(templates[name], vals[0], vals[1], vals[2]); break;
                    case 4: str = string.Format(templates[name], vals[0], vals[1], vals[2], vals[3]); break;
                }

                return str;
            }

            if (e.NodeType == XmlNodeType.Text)
            {
                return ((XText)e).Value.FormatSummary();
            }

            return "";
        }

        internal static string ToMarkDown(this IEnumerable<XNode> es)
        {
            return es.Aggregate("", (current, x) => current + x.ToMarkDown());
        }

        internal static string ToMarkDownList(this XNode e)
        {

            var templates = new Dictionary<string, string>
                {
                    {"doc", ""},
                    {"type", "| [{0}](#{1}) {2}"},
                    {"field", ""},
                    {"property", "| [{0}](#{1}) {2}"},
                    {"globalProperty", "#### {0}\n\n{1}\n\n---\n"},
                    {"method", "| [{0}](#{1}) {2}"},
                    {"event", "| [{0}](#{1}) {2}"},
                    {"summary", "| {0} |\n"},
                    {"remarks", ""},
                    {"example", "_C# code_\n\n```c#\n{0}\n```\n\n"},
                    {"seePage", "[[{1}|{0}]]"},
                    {"seeAnchor", "[{1}]({0})"},
                    {"param", "" },
                    {"exception", "" },
                    {"returns", ""},
                    {"externalLink", "[{1}]({0})"},
                    {"code", "`{0}`"},
                    {"embed", "`<embed></embed>`"},
                    {"bold", "**{0}**"},
                    {"italic", "__{0}__"},
                    {"body", "_Java Script_\n\n``` js\n<body>\n   {0}\n<\body>\n```\n\n"},
                    {"none", ""}
                };

            var d = new Func<string, XElement, string[]>((att, node) => new[]
                {
                    node.Attribute(att).Value.FormatParameters(),
                    node.Attribute(att).Value.FormatLink(),
                    node.Nodes().ToMarkDownList()
                });

            var methods = new Dictionary<string, Func<XElement, IEnumerable<string>>>
                {
                    {"doc", x=> new[]{
                        x.Element("assembly").Element("name").Value,
                        x.Element("members").Elements("member").ToMarkDownList()
                    }},
                    {"type", x=>d("name", x)},
                    {"field", x=> d("name", x)},
                    {"property", x=> d("name", x)},
                    {"method",x=>d("name", x)},
                    {"event", x=>d("name", x)},
                    {"summary", x=> new[]{ x.Nodes().ToMarkDownList() }},
                    {"remarks", x => new[]{x.Nodes().ToMarkDownList()}},
                    {"example", x => new[]{x.Value.ToCodeBlock()}},
                    {"seePage", x=> d("cref", x) },
                    {"seeAnchor", x=> { var xx = d("cref", x); xx[0] = xx[0].ToLower(); return xx; }},
                    {"param", x => d("name", x) },
                    {"exception", x => d("cref", x) },
                    {"returns", x => new[]{x.Nodes().ToMarkDownList()}},
                    {"externalLink", x => d("href", x) },
                    {"code", x => new[]{x.Nodes().ToMarkDownList()}},
                    {"embed", x => new[]{x.Nodes().ToMarkDownList()}},
                    {"bold", x => new[]{x.Nodes().ToMarkDownList()}},
                    {"italic", x => new[]{x.Nodes().ToMarkDownList()}},
                    {"body", x => new[]{x.Value.ToCodeBlock()}},
                    {"none", x => new string[0]}
                };

            string name;
            if (e.NodeType == XmlNodeType.Element)
            {
                var el = (XElement)e;
                name = el.Name.LocalName;

                if (name == "member")
                {
                    switch (el.Attribute("name").Value[0])
                    {
                        case 'F': name = "field"; break;
                        case 'P': name = "property"; break;
                        case 'T': name = "type"; break;
                        case 'E': name = "event"; break;
                        case 'M': name = "method"; break;
                        default: name = "none"; break;
                    }
                }
                if (name == "see")
                {
                    var anchor = el.Attribute("cref").Value.StartsWith("!:#");
                    name = anchor ? "seeAnchor" : "seePage";
                }
                if (name == "code")
                {
                    name = "code";
                }
                if (name == "embed")
                {
                    name = "embed";
                }
                if (name == "a")
                {
                    name = "externalLink";
                }
                if (name == "strong")
                {
                    name = "bold";
                }
                if (name == "i")
                {
                    name = "italic";
                }
                if (name == "body")
                {
                    name = "body";
                }
                if (name == "typeparam")
                {
                    name = "param";
                }
                var vals = methods[name](el).ToArray();
                string str = "";
                switch (vals.Length)
                {
                    case 1: str = string.Format(templates[name], vals[0]); break;
                    case 2: str = string.Format(templates[name], vals[0], vals[1]); break;
                    case 3: str = string.Format(templates[name], vals[0], vals[1], vals[2]); break;
                    case 4: str = string.Format(templates[name], vals[0], vals[1], vals[2], vals[3]); break;
                }

                return str;
            }

            if (e.NodeType == XmlNodeType.Text)
            {
                return ((XText)e).Value.FormatSummary();
            }

            return "";
        }

        internal static string ToMarkDownList(this IEnumerable<XNode> es)
        {
            return es.Aggregate("", (current, x) => current + x.ToMarkDownList());
        }

        internal static string FormatParameters(this string s)
        {
            return s.Replace("(", " (").Replace(",", ", "); 
        }

        internal static string FormatSummary(this string text)
        {
            if (text.Length > 0 && text[0] == '\n')
            {
                text = text.Substring(1);
                var lines = text.Split('\n');
                // get first non whitespace
                var shift = lines[0].TakeWhile(c => char.IsWhiteSpace(c)).Count();
                // Shift all the lines to the left this amount.
                for (var s = 0; s < lines.Length; s++)
                {
                    if (lines[s].Length > shift)
                        lines[s] = lines[s].Substring(shift);
                }
                text = string.Join("</br>", lines);

            }
            return text.Replace("\n", "</br>");

        }

        internal static string FormatLink(this string text)
        {

            var link = new string(text.ToCharArray()
                .Where(c => !Char.IsPunctuation(c))
                .ToArray());

            return link.ToLower();

        }

        static string ToCodeBlock(this string s)
        {
            var lines = s.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            var blank = lines[0].TakeWhile(x => x == ' ').Count() - 4;
            return string.Join("\n",lines.Select(x => new string(x.SkipWhile((y, i) => i < blank).ToArray())));
        }
    }
}