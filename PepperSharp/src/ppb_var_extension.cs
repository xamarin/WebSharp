/* Copyright (c) 2016 Xamarin. */

/* NOTE: this file contains code that needs to have some hand holding instead of auto-generated from IDL */
/* From ppb_var.idl  */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {

    /**
 * @addtogroup Structs
 * @{
 */
    /**
     * The PP_VarValue union stores the data for any one of the types listed
     * in the PP_VarType enum.
     */

    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public struct PP_VarValue
    {
        /**
         * If <code>type</code> is <code>PP_VARTYPE_BOOL</code>,
         * <code>as_bool</code> represents the value of this <code>PP_Var</code> as
         * <code>PP_Bool</code>.
         */
        [FieldOffset(0)]
        public PP_Bool as_bool;
        /**
         * If <code>type</code> is <code>PP_VARTYPE_INT32</code>,
         * <code>as_int</code> represents the value of this <code>PP_Var</code> as
         * <code>int32_t</code>.
         */
        [FieldOffset(0)]
        public int as_int;
        /**
         * If <code>type</code> is <code>PP_VARTYPE_DOUBLE</code>,
         * <code>as_double</code> represents the value of this <code>PP_Var</code>
         * as <code>double</code>.
         */
        [FieldOffset(0)]
        public double as_double;
        /**
         * If <code>type</code> is <code>PP_VARTYPE_STRING</code>,
         * <code>PP_VARTYPE_OBJECT</code>, <code>PP_VARTYPE_ARRAY</code>,
         * <code>PP_VARTYPE_DICTIONARY</code>, <code>PP_VARTYPE_ARRAY_BUFFER</code>,
         * or <code>PP_VARTYPE_RESOURCE</code>, <code>as_id</code> represents the
         * value of this <code>PP_Var</code> as an opaque handle assigned by the
         * browser. This handle is guaranteed never to be 0, so a module can
         * initialize this ID to 0 to indicate a "NULL handle."
         */
        [FieldOffset(0)]
        public long as_id;
    };


    /**
     * PPB_Var API
     */
    public static partial class PPB_Var
    {
        [DllImport("PepperPlugin", EntryPoint = "PPB_Var_VarToUtf8")]
        extern static IntPtr _VarToUtf8(PP_Var var, out uint len);

        /**
         * VarToUtf8() converts a string-type var to a char* encoded in UTF-8. This
         * string is NOT NULL-terminated. The length will be placed in
         * <code>*len</code>. If the string is valid but empty the return value will
         * be non-NULL, but <code>*len</code> will still be 0.
         *
         * If the var is not a string, this function will return NULL and
         * <code>*len</code> will be 0.
         *
         * The returned buffer will be valid as long as the underlying var is alive.
         * If the instance frees its reference, the string will be freed and the
         * pointer will be to arbitrary memory.
         *
         * @param[in] var A PP_Var struct containing a string-type var.
         * @param[in,out] len A pointer to the length of the string-type var.
         *
         * @return A char* encoded in UTF-8.
         */
        public static string VarToUtf8(PP_Var var, out uint len)
        {
            return Marshal.PtrToStringAnsi(_VarToUtf8(var, out len));
        }
    }
}
/**
 * @}
 */



