using System;
using System.Reflection;

public static class MonoEmbedding
{
    static Int64 MinDateTimeTicks = 621355968000000000; // (new DateTime(1970, 1, 1, 0, 0, 0)).Ticks;

    // dummy Main so we can exec from mono
    static public int Main()
    {
        return 0;
    }

    static public Exception NormalizeException(Exception e)
    {
        AggregateException aggregate = e as AggregateException;
        if (aggregate != null && aggregate.InnerExceptions.Count == 1)
        {
            e = aggregate.InnerExceptions[0];
        }
        else {
            TargetInvocationException target = e as TargetInvocationException;
            if (target != null && target.InnerException != null)
            {
                e = target.InnerException;
            }
        }

        return e;
    }

    static public DateTime CreateDateTime(double ticks)
    {
        return new DateTime((Int64)ticks * 10000 + MinDateTimeTicks, DateTimeKind.Utc);
    }

    static public Func<Object, Object> GetFunc(string assemblyFile, string typeName, string methodName)
    {
        Console.WriteLine($"CS::GetFunc: AssemblyFile: {assemblyFile} | TypeName: {typeName} | MethodName: {methodName} ");
        try {
            var assembly = Assembly.Load(assemblyFile);
            var wrap = ClrFuncReflectionWrap.Create(assembly, typeName, methodName);
            Console.WriteLine("CS::GetFunc::End");
            return new Func<Object, Object>(wrap.Call);
        }
        catch (Exception exc)
        {
            Console.WriteLine($"CS::GetFunc::Exception - {exc.Message}");
            throw exc;
        }

    }
    

    static public string ObjectToString(object o)
    {
        return o.ToString();
    }

    static public double Int64ToDouble(long i64)
    {
        return (double)i64;
    }

    static public string TryConvertPrimitiveOrDecimal(object obj)
    {
        Type t = obj.GetType();
        if (t.IsPrimitive || typeof(Decimal) == t)
        {
            IConvertible c = obj as IConvertible;
            return c == null ? obj.ToString() : c.ToString();
        }
        else 
        {
            return null;
        }
    }
}
