using System;
using PepperSharp;

namespace HelloWorld
{
    public class HelloWorld : Instance
    {

        ~HelloWorld()
        {
            System.Console.WriteLine("HelloWorld destructed");
        }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            //LogToConsoleWithSource(PPLogLevel.Log, "HellowWorld.dll", "HelloWorld from PepperSharp using C#");
            return true;
        }
    }
}
