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

        public virtual bool Init(int argc, string[] argn, string[] argv)
        {
            return true;
        }

    }
}
