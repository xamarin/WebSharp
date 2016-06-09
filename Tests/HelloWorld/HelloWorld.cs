using System;
using Pepper;

namespace HelloWorld
{
    public class HelloWorld : PPInstance
    {

        ~HelloWorld()
        {
            System.Console.WriteLine("HelloWorld destructed");
        }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            LogToConsoleWithSource(PP_LogLevel.PP_LOGLEVEL_LOG, "HellowWorld.dll", "HelloWorld from PepperSharp using C#");
            return true;
        }
    }
}
