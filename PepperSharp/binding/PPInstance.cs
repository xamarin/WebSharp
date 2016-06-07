using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pepper
{
    public partial class PPInstance : PepperBase
    {
        protected PPInstance(IntPtr handle) : base(handle) { }

        public virtual bool Init(int argc, string[] argn, string[] argv)
        {
            return true;
        }
    }
}
