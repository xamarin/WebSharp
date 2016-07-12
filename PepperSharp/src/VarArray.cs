using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    public partial class VarArray : Var
    {

        public VarArray() : base(Var.Empty)
        {
            ppvar = PPBVarDictionary.Create();
        }

        public VarArray(Var var) : base(var)
        {
            if (!var.IsArray)
                ppvar = Var.Empty;
        }

        /// <summary>
        /// Retrieves the length of the ArrayBuffer that is referenced
        /// </summary>
        public uint Length
        {
            get
            {
                if (IsArray)
                    return PPBVarArray.GetLength(ppvar);
                return 0;
            }

            set
            {
                if (IsArray)
                    PPBVarArray.SetLength(ppvar, value);
            }
        }

        public Var this[uint index]
        {
            get
            {
                return Get(index);
            }
            set
            {
                if (IsArray)
                    Set(index, value);
            }
        }            

        public Var Get(uint index)
        {
            if (IsArray)
                return PPBVarArray.Get(ppvar, index);
            return Var.Empty;
        }

        public bool Set(uint index, Var value)
        {
            if (IsArray)
                return PPBVarArray.Set(ppvar, index, value) == PPBool.True;
            return false;
        }

        public bool Set(uint index, object value)
        {
            if (IsArray)
                return PPBVarArray.Set(ppvar, index, new Var(value)) == PPBool.True;
            return false;
        }
    }
}
