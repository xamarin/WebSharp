//
// MouseButtons.cs
//


using System;

using WebSharpJs.Script;

namespace WebSharpJs.DOM
{

    [Flags]
    public enum MouseButtons
    {
        None = 0,
        Left = 1,
        Right = 2,
        Middle = 4
    }

}