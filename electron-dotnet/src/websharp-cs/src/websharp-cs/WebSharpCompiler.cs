using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

public class WebSharpCompiler
{
    static readonly Regex referenceRegex = new Regex(@"^[\ \t]*(?:\/{2})?\#r[\ \t]+""([^""]+)""", RegexOptions.Multiline);
    static readonly Regex usingRegex = new Regex(@"^[\ \t]*(using[\ \t]+[^\ \t]+[\ \t]*\;)", RegexOptions.Multiline);
    static readonly bool debuggingEnabled = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSHARP_CS_DEBUG"));
    static readonly bool debuggingSelfEnabled = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSHARP_CS_DEBUG_SELF"));
    static readonly bool cacheEnabled = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSHARP_CS_CACHE"));
    static Dictionary<string, Dictionary<string, Assembly>> referencedAssemblies = new Dictionary<string, Dictionary<string, Assembly>>();
    static Dictionary<string, Func<object, Task<object>>> funcCache = new Dictionary<string, Func<object, Task<object>>>();
    static readonly string websharpLocation = Directory.GetParent(Path.GetDirectoryName(typeof(WebSharpCompiler).Assembly.Location)).FullName + Path.DirectorySeparatorChar + "WebSharpJs.dll";
    static readonly string frameworkPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

    static WebSharpCompiler()
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
    }

    static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        Assembly result = null;
        Dictionary<string, Assembly> requesting;

        if (args.RequestingAssembly != null)
        {
            if (referencedAssemblies.TryGetValue(args.RequestingAssembly.FullName, out requesting))
            {
                requesting.TryGetValue(args.Name, out result);
            }
        }

        return result;
    }

    public Func<object, Task<object>> CompileFunc(IDictionary<string, object> parameters)
    {
        string source = (string)parameters["source"];
        string lineDirective = string.Empty;
        string fileName = null;
        int lineNumber = 1;

        // read source from file
        if (source.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase)
            || source.EndsWith(".csx", StringComparison.InvariantCultureIgnoreCase))
        {
            // retain fileName for debugging purposes
            if (debuggingEnabled)
            {
                fileName = source;
                Console.WriteLine($"Reading from source file: {Path.GetFullPath(fileName)}");
            }

            source = File.ReadAllText(source);
        }

        if (debuggingSelfEnabled)
        {
            Console.WriteLine("Func cache size: " + funcCache.Count);
        }

        var originalSource = source;
        if (funcCache.ContainsKey(originalSource))
        {
            if (debuggingSelfEnabled)
            {
                Console.WriteLine("Serving func from cache.");
            }

            return funcCache[originalSource];
        }
        else if (debuggingSelfEnabled)
        {
            Console.WriteLine("Func not found in cache. Compiling.");
        }

        // add assembly references provided explicitly through parameters
        List<string> references = new List<string>();
        object v;
        if (parameters.TryGetValue("references", out v))
        {
            if (debuggingSelfEnabled)
            {
                Console.WriteLine($"Additional references were specified.");
            }

            foreach (object reference in (object[])v)
            {
                if (debuggingSelfEnabled)
                {
                    Console.WriteLine($"Adding reference : {reference} ");
                }
                references.Add((string)reference);
            }
        }

        // add assembly references provided in code as [//]#r "assemblyname" lines
        Match match = referenceRegex.Match(source);
        if (debuggingSelfEnabled && match.Success)
            Console.WriteLine($"Assembly references provided in code.");
        while (match.Success)
        {
            if (debuggingSelfEnabled)
                Console.WriteLine($"Adding reference: {match.Groups[1].Value}");
            references.Add(match.Groups[1].Value);
            source = source.Substring(0, match.Index) + source.Substring(match.Index + match.Length);
            match = referenceRegex.Match(source);
        }

        if (debuggingEnabled)
        {
            object jsFileName;
            if (parameters.TryGetValue("jsFileName", out jsFileName))
            {
                fileName = (string)jsFileName;
                lineNumber = (int)parameters["jsLineNumber"];
            }
            
            if (!string.IsNullOrEmpty(fileName)) 
            {
                lineDirective = string.Format("#line {0} \"{1}\"\n", lineNumber, fileName);
            }
        }

        // try to compile source code as a class library
        Assembly assembly;
        string errorsClass;
        if (!this.TryCompile(lineDirective + source, 
                            references, 
                            out errorsClass, out assembly, fileName))
        {
            // try to compile source code as an async lambda expression

            // extract using statements first
            string usings = "";
            match = usingRegex.Match(source);
            while (match.Success)
            {
                usings += match.Groups[1].Value;
                source = source.Substring(0, match.Index) + source.Substring(match.Index + match.Length);
                match = usingRegex.Match(source);
            }

            string errorsLambda;
            source = 
                usings + "using System;\n"
                + "using System.Threading.Tasks;\n"
                + "public class Startup {\n"
                + "    public async Task<object> Invoke(object ___input) {\n"
                + lineDirective
                + "        Func<object, Task<object>> func = " + source + ";\n"
                + "#line hidden\n"
                + "        return await func(___input);\n"
                + "    }\n"
                + "}";

            if (debuggingSelfEnabled)
            {
                Console.WriteLine("WebSharp-cs trying to compile async lambda expression:");
            }

            if (!TryCompile(source, references, out errorsLambda, out assembly))
            {
                throw new InvalidOperationException(
                    "Unable to compile C# code.\n----> Errors when compiling as a CLR library:\n"
                    + errorsClass
                    + "\n----> Errors when compiling as a CLR async lambda expression:\n"
                    + errorsLambda);
            }
        }

        // store referenced assemblies to help resolve them at runtime from AppDomain.AssemblyResolve
        referencedAssemblies[assembly.FullName] = new Dictionary<string, Assembly>();
        foreach (var reference in references)
        {
            try
            {
                var referencedAssembly = Assembly.UnsafeLoadFrom(reference);
                referencedAssemblies[assembly.FullName][referencedAssembly.FullName] = referencedAssembly;
            }
            catch
            {
                // empty - best effort
            }
        }

        // extract the entry point to a class method
        Type startupType = assembly.GetType((string)parameters["typeName"], true, true);
        object instance = Activator.CreateInstance(startupType, false);
        MethodInfo invokeMethod = startupType.GetMethod((string)parameters["methodName"], BindingFlags.Instance | BindingFlags.Public);
        if (invokeMethod == null)
        {
            throw new InvalidOperationException("Unable to access CLR method to wrap through reflection. Make sure it is a public instance method.");
        }

        // create a Func<object,Task<object>> delegate around the method invocation using reflection
        Func<object,Task<object>> result = (input) => 
        {
            return (Task<object>)invokeMethod.Invoke(instance, new object[] { input });
        };

        if (cacheEnabled)
        {
            funcCache[originalSource] = result;
        }

        return result;
    }

    bool TryCompile(string source, List<string> references, out string errors, out Assembly assembly, string path = null)
    {
        bool result = false;
        assembly = null;
        errors = null;
        var srcPath = (debuggingEnabled && !string.IsNullOrEmpty(path)) ? Path.GetFullPath(path) : string.Empty;
        if (debuggingSelfEnabled)
            Console.WriteLine($"Adding source path to parse: {srcPath}");

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source, path: srcPath, encoding: System.Text.Encoding.UTF8 );

        string assemblyName = Path.GetRandomFileName();

        if (debuggingSelfEnabled)
            Console.WriteLine($"Loading WebSharpJs.dll from {websharpLocation}");

        // default references
        MetadataReference[] defaultReferences = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),  // mscorelib.dll
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),  // System.Core
            MetadataReference.CreateFromFile(typeof(RuntimeBinderException).Assembly.Location), // Microsoft.CSharp
            MetadataReference.CreateFromFile(Path.Combine(frameworkPath, "System.dll")) // System.dll
        };

        var metadataReferences = new List<MetadataReference>(defaultReferences);

        // Add a reference to WebSharpJs.dll
        references.Add(websharpLocation);

        foreach (var reference in references)
        {
            try
            {
                metadataReferences.Add(MetadataReference.CreateFromFile(reference));
            }
            catch
            {
                // Try to load from assembly name
                if (debuggingSelfEnabled)
                    System.Console.WriteLine($"Could not find: {reference}.  Trying to load from AssemblyName.");

                try
                {
                    metadataReferences.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName(reference)).Location));
                }
                catch {
                    // Try to load from runtime directory
                    if (debuggingSelfEnabled)
                        System.Console.WriteLine($"Could not find: {reference}.  Trying to load from runtime library.");

                    try
                    {
                        var frameworkLib = Path.Combine(frameworkPath, reference);
                        metadataReferences.Add(MetadataReference.CreateFromFile(frameworkLib));
                    }
                    catch (Exception exc)
                    {
                        if (debuggingSelfEnabled)
                            System.Console.WriteLine($"Could not load reference {reference}.  Message: {exc.Message}");
                    }

                }
            }
        }

        //if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EDGE_CS_TEMP_DIR")))
        //{
        //    parameters.TempFiles = new TempFileCollection(Environment.GetEnvironmentVariable("EDGE_CS_TEMP_DIR"));
        //}
        if (debuggingSelfEnabled)
        {
            Console.WriteLine($"OptimizationLevel: {((debuggingEnabled) ? OptimizationLevel.Debug : OptimizationLevel.Release)}");
        }
        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithOptimizationLevel((debuggingEnabled) ? OptimizationLevel.Debug : OptimizationLevel.Release);

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: metadataReferences.ToArray(),
            options: compilationOptions);

        using (var ms = new MemoryStream())
        {
            EmitResult emitResult = compilation.Emit(ms);

            if (!emitResult.Success)
            {
                IEnumerable<Diagnostic> failures = emitResult.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (Diagnostic diagnostic in failures)
                {
                    if (errors == null)
                    {
                        errors = $"{diagnostic.Id}: {diagnostic.GetMessage()}";
                        //Console.WriteLine(errors);
                    }
                    else
                    {
                        errors += "\n" + $"{diagnostic.Id}: {diagnostic.GetMessage()}";
                    }
                }
            }
            else
            {
                ms.Seek(0, SeekOrigin.Begin);
                assembly = Assembly.Load(ms.ToArray());
                result = true;
            }

            return result;
        }
    }
}
