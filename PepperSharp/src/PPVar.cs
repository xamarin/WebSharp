using System;

namespace PepperSharp
{
    /// <summary>
    /// This class provides helper methods around a PP_Var
    /// </summary>
    public sealed class PPVar : IDisposable
    {

        internal PP_Var ppvar = new PP_Var();
        public static readonly PPVar Empty = new PPVar(PP_VarType.Null);

        // |is_managed_| indicates if the instance manages |var_|.
        // You need to check if |var_| is refcounted to call Release().
        internal bool isManaged;

        private bool disposed = false;

        #region Constructors

        private PPVar(PP_VarType type)
        {
            ppvar.type = type;
        }

        public PPVar(object var)
        {
            if (var is int || var is uint)
            {
                ppvar.type = PP_VarType.Int32;
                ppvar.value.as_int = (int)var;
                isManaged = true;
            }
            else if (var is string)
            {
                ppvar = VarFromUtf8Helper((string)var);
                isManaged = true;
            }
            else if (var is bool)
            {
                ppvar.type = PP_VarType.Bool;
                ppvar.value.as_bool = (bool)var ? PP_Bool.PP_TRUE : PP_Bool.PP_FALSE;
                isManaged = true;
            }
            else if (var is double)
            {
                ppvar.type = PP_VarType.Double;
                ppvar.value.as_double = (double)var;
                isManaged = true;
            }
            // Note: You may see precision differences 
            else if (var is float)
            {
                ppvar.type = PP_VarType.Double;
                ppvar.value.as_double = Convert.ToDouble((float)var);
                isManaged = true;
            }

            else
            {
                ppvar.type = PP_VarType.Undefined;
                isManaged = true;
            }
        }

        public PPVar(PP_Var var)
        {
            ppvar = var;
            isManaged = true;
            if (NeedsRefcounting(ppvar))
            {
                PPBVar.AddRef(ppvar);
            }
        }

        public PPVar(PPVar other)
        {
            ppvar = other.ppvar;
            isManaged = true;
            if (NeedsRefcounting(ppvar))
            {
                PPBVar.AddRef(ppvar);
            }
        }
        #endregion

        #region Implement IDisposable.

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free up the reference counters.
                if (NeedsRefcounting(ppvar) && isManaged)
                {
                    PPBVar.Release(ppvar);
                }

                disposed = true;
            }
        }

        ~PPVar()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion

        // Technically you can call AddRef and Release on any Var, but it may involve
        // cross-process calls depending on the plugin. This is an optimization so we
        // only do refcounting on the necessary objects.
        bool NeedsRefcounting(PP_Var var)
        {
            return var.type > PP_VarType.Double;
        }

        // This helper function uses the latest available version of VarFromUtf8. Note
        // that version 1.0 of this method has a different API to later versions.
        public static PP_Var VarFromUtf8Helper(string utf8_str)
        {
            return PPBVar.VarFromUtf8(utf8_str, (uint)utf8_str.Length);
        }

        public bool AsBoolean()
        {
            if (!IsBoolean)
                return false;
            return ppvar.value.as_bool == PP_Bool.PP_TRUE;
        }

        public int AsInt()
        {
            if (IsInt)
                return ppvar.value.as_int;
            if (IsDouble)
                return (int)(ppvar.value.as_double);
            return 0;
        }

        public double AsDouble()
        {
            if (IsDouble)
                return ppvar.value.as_double;
            if (IsInt)
                return ppvar.value.as_int;
            return 0.0;
        }

        public string AsString()
        {
            if (!IsString)
            {
                return string.Empty;
            }

            uint len = 0;

            return PPBVar.VarToUtf8(ppvar, out len);
        }

        public PP_Var AsPP_Var()
        {
            return ppvar;
        }

        public object AsObject()
        {
            if (IsUndefined)
            {
                return "Var(UNDEFINED)";
            }
            else if (IsNull)
            {
                return "Var(NULL)";
            }
            else if (IsBoolean)
            {
                return AsBoolean();
            }
            else if (IsInt)
            {
                return AsInt();
            }
            else if (IsDouble)
            {
                return AsDouble();
            }
            else if (IsString)
            {
                return AsString();
            }
            else if (IsObject)
            {
                return "Var(OBJECT)";
            }
            else if (IsArray)
            {
                return "Var(ARRAY)";
            }
            else if (IsDictionary)
            {
                return "Var(DICTIONARY)";
            }
            else if (IsArrayBuffer)
            {
                return "Var(ARRAY_BUFFER)";
            }
            else if (IsResource)
            {
                return "Var(RESOURCE)";
            }
            return string.Empty;

        }

        // Prints a debug string.
        public string DebugString()
        {
            if (IsUndefined)
            {
                return "Var(UNDEFINED)";
            }
            else if (IsNull)
            {
                return "Var(NULL)";
            }
            else if (IsBoolean)
            {
                return AsBoolean() ? "Var(true)" : "Var(false)";
            }
            else if (IsInt)
            {
                return $"Var({AsInt()})";
            }
            else if (IsDouble)
            {
                return $"Var({AsDouble()})";
            }
            else if (IsString)
            {
                return $"Var<'{AsString()}'>";
            }
            else if (IsObject)
            {
                return "Var(OBJECT)";
            }
            else if (IsArray)
            {
                return "Var(ARRAY)";
            }
            else if (IsDictionary)
            {
                return "Var(DICTIONARY)";
            }
            else if (IsArrayBuffer)
            {
                return "Var(ARRAY_BUFFER)";
            }
            else if (IsResource)
            {
                return "Var(RESOURCE)";
            }
            return string.Empty;
        }

        public override string ToString()
        {
            return AsObject().ToString();
        }

        /// This function determines if this <code>Var</code> is an undefined value.
        ///
        /// @return true if this <code>Var</code> is undefined, otherwise false.
        public bool IsUndefined { get { return ppvar.type == PP_VarType.Undefined; } }

        /// This function determines if this <code>Var</code> is a null value.
        ///
        /// @return true if this <code>Var</code> is null, otherwise false.
        public bool IsNull { get { return ppvar.type == PP_VarType.Null; } }

        /// This function determines if this <code>Var</code> is a bool value.
        ///
        /// @return true if this <code>Var</code> is a bool, otherwise false.
        public bool IsBoolean { get { return ppvar.type == PP_VarType.Bool; } }

        /// This function determines if this <code>Var</code> is a string value.
        ///
        /// @return true if this <code>Var</code> is a string, otherwise false.
        public bool IsString { get { return ppvar.type == PP_VarType.String; } }

        /// This function determines if this <code>Var</code> is an object.
        ///
        /// @return true if this <code>Var</code> is an object, otherwise false.
        public bool IsObject { get { return ppvar.type == PP_VarType.Object; } }

        /// This function determines if this <code>Var</code> is an array.
        ///
        /// @return true if this <code>Var</code> is an array, otherwise false.
        public bool IsArray { get { return ppvar.type == PP_VarType.Array; } }

        /// This function determines if this <code>Var</code> is a dictionary.
        ///
        /// @return true if this <code>Var</code> is a dictionary, otherwise false.
        public bool IsDictionary { get { return ppvar.type == PP_VarType.Dictionary; } }

        /// This function determines if this <code>Var</code> is a resource.
        ///
        /// @return true if this <code>Var</code> is a resource, otherwise false.
        public bool IsResource { get { return ppvar.type == PP_VarType.Resource; } }

        /// This function determines if this <code>Var</code> is an integer value.
        /// The <code>is_int</code> function returns the internal representation.
        /// The JavaScript runtime may convert between the two as needed, so the
        /// distinction may not be relevant in all cases (int is really an
        /// optimization inside the runtime). So most of the time, you will want
        /// to check is_number().
        ///
        /// @return true if this <code>Var</code> is an integer, otherwise false.
        public bool IsInt { get { return ppvar.type == PP_VarType.Int32; } }

        /// This function determines if this <code>Var</code> is a double value.
        /// The <code>is_double</code> function returns the internal representation.
        /// The JavaScript runtime may convert between the two as needed, so the
        /// distinction may not be relevant in all cases (int is really an
        /// optimization inside the runtime). So most of the time, you will want to
        /// check is_number().
        ///
        /// @return true if this <code>Var</code> is a double, otherwise false.
        public bool IsDouble { get { return ppvar.type == PP_VarType.Double; } }

        /// This function determines if this <code>Var</code> is a number.
        ///
        /// @return true if this <code>Var</code> is an int32_t or double number,
        /// otherwise false.
        public bool IsNumber
        {
            get
            {
                return ppvar.type == PP_VarType.Int32 ||
                 ppvar.type == PP_VarType.Double;
            }
        }

        /// This function determines if this <code>Var</code> is an ArrayBuffer.
        public bool IsArrayBuffer { get { return ppvar.type == PP_VarType.Array_buffer; } }

        public static implicit operator PP_Var(PPVar var)
        {
            return var.AsPP_Var();
        }

    }
}
