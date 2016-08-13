using System;
using PepperSharp;

namespace HelloWorld
{
    public class HelloWorld : Instance
    {

        public HelloWorld(IntPtr handle) : base(handle)
        {
            Initialize += OnInitialize;
        }

        ~HelloWorld()
        {
            System.Console.WriteLine("HelloWorld destructed");
        }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsoleWithSource(PPLogLevel.Log, "HellowWorld.dll", "HelloWorld from PepperSharp using C#");
        }
    }
}
