using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    public partial class VarArrayBuffer : Var
    {
        byte[] dataMap; // Managed contents of the unmanaged data copied
        IntPtr dataPtr; // Pointer to the ArrayBuffer unmanaged data
        bool isMapped; // Whether or not the data is mapped.

        public VarArrayBuffer (Var var) : base(var)
        {
            if (!var.IsArrayBuffer)
                ppvar = Var.Empty;
        }

        /// <summary>
        /// Creates a VarArrayBuffer of a particular size.
        /// </summary>
        /// <param name="size"></param>
        public VarArrayBuffer(uint size) : base(PPVarType.Undefined)
        {

            ppvar = PPBVarArrayBuffer.Create(size);
            isManaged = true;
        }

        /// <summary>
        /// Retrieves the length of the ArrayBuffer that is referenced
        /// </summary>
        public uint ByteLength
        {
            get
            {
                uint numBytes = UInt32.MaxValue;
                if (IsArrayBuffer)
                    PPBVarArrayBuffer.ByteLength(ppvar, out numBytes);
                return numBytes;
            }
        }

        /// <summary>
        /// Map() maps the <code>ArrayBuffer</code> in to the module's address space
        /// and returns a pointer to the beginning of the buffer for the given
        /// <code>ArrayBuffer PP_Var</code>. ArrayBuffers are copied when transmitted,
        /// so changes to the underlying memory are not automatically available to
        /// the embedding page.
        ///
        /// Note that calling Map() can be a relatively expensive operation. Use care
        /// when calling it in performance-critical code. For example, you should call
        /// it only once when looping over an <code>ArrayBuffer</code>.
        ///
        /// <strong>Example:</strong>
        ///
        /// @code
        /// char* data = (char*)(array_buffer_if.Map(array_buffer_var));
        /// uint32_t byte_length = 0;
        /// PP_Bool ok = array_buffer_if.ByteLength(array_buffer_var, &byte_length);
        /// if (!ok)
        ///   return DoSomethingBecauseMyVarIsNotAnArrayBuffer();
        /// for (uint32_t i = 0; i < byte_length; ++i)
        ///   data[i] = 'A';
        /// @endcode
        /// </summary>
        /// <returns>
        /// A byte array with a copy of the data that is contained in unmanaged memory.
        /// </returns>
        public byte[] Map()
        {
            dataPtr = PPBVarArrayBuffer.Map(ppvar);
            var numBytes = ByteLength;
            dataMap = new byte[numBytes];
            Marshal.Copy(dataPtr, dataMap, 0, dataMap.Length);
            isMapped = true;
            return dataMap;
        }

        /// <summary>
        /// Unmap() unmaps the given <code>ArrayBuffer</code> var from the module
        /// address space.  Use this if you want to save memory but might want to call
        /// Map() to map the buffer again later.  The<code>PP_Var</code> remains valid
        /// and should still be released using <code>PPB_Var</code> when you are done
        /// with the<code> ArrayBuffer</code>.
        /// </summary>
        public void UnMap()
        {
            PPBVarArrayBuffer.Unmap(ppvar);
            isMapped = false;
            dataMap = null;
            dataPtr = IntPtr.Zero;
        }

        /// <summary>
        /// When doing a Map() the information that is contained in the pointer returned
        /// is copied from the unmanaged data to a byte[] array.  If the byte array that is
        /// returned is modified and needs to be updated to the managed module memory space then
        /// call Flush() will copy this data to the unmanaged memory.
        /// 
        /// This will not work if UnMap() is called before.
        /// 
        /// Exampe usage:
        /// 
        /// var arrayBuffer = new VarArrayBuffer(size);
        /// var data = arrayBuffer.Map();
        /// for (int i = 0; i<size; ++i)
        ///     data[i] = (byte)message[i];
        /// arrayBuffer.Flush();
        /// arrayBuffer.UnMap();
        /// 
        /// </summary>
        public void Flush()
        {
            if (isMapped)
                Marshal.Copy(dataMap, 0, dataPtr, dataMap.Length);
        }

    }
}
