using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;

namespace Pepper
{
    public partial class PPInstance : PepperBase
    {
        protected PPInstance() { throw new PlatformNotSupportedException("Can not create an instace of PPInstance"); }

        public virtual bool Init(int argc, string[] argn, string[] argv)
        {
            return true;
        }

    }
}
