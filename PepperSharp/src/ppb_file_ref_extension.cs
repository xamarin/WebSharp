using System;
using System.Text;

namespace PepperSharp
{
    public static partial class PPBFileRef
    {
        public static PPResource Create(PPResource file_system, string path)
        {
            return PPBFileRef.Create(file_system, Encoding.UTF8.GetBytes(path));
        }
    }
}
