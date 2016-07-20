using System;

namespace PepperSharp
{
    public partial class VarDictionary : Var
    {
        public VarDictionary () : base(Var.Empty)
        {
            ppvar = PPBVarDictionary.Create();
        }

        public VarDictionary(Var var) : base(var)
        {
            if (!var.IsDictionary)
                ppvar = Var.Empty;
        }

        public Var Get(Var key)
        {
            return PPBVarDictionary.Get(ppvar, key);
        }

        public Var Get(object key)
        {
            return Get(new Var(key));
        }

        public bool Set(Var key, Var value)
        {
            var setResult = PPBVarDictionary.Set(ppvar, key, value);
            return setResult == PPBool.True;
        }

        public bool Set(object key, object value)
        {
            return Set(new Var(key), new Var(value));
        }

        public void Delete (Var key)
        {
            PPBVarDictionary.Delete(ppvar, key);
        }

        public void Delete(object key)
        {
            Delete(new Var( key));
        }

        public bool HasKey(Var key)
        {
            var hasKey = PPBVarDictionary.HasKey(ppvar, key);
            return hasKey == PPBool.True;
        }

        public bool HasKey(object key)
        {
            return HasKey(new Var(key));
        }

        VarArray GetKeys() {

            var result = (Var)PPBVarDictionary.GetKeys(ppvar);
            if (result.IsArray)
                return (VarArray)result;
            else
                return new VarArray();
        }

    }
}
