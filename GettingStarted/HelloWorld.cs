using System;

using PepperSharp;

namespace GettingStarted
{
    public class HelloWorld : Instance
    {
        public override bool Init(int argc, string[] argn, string[] argv)
        {
            LogToConsoleWithSource(PPLogLevel.Log, "GettingStarted.HelloWorld", "HelloWorld from C#");
            return true;
        }
    }
}
