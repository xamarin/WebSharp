using System;

using PepperSharp;

namespace GettingStarted
{
    public class HelloWorld : Instance
    {
        public HelloWorld (IntPtr handle) : base(handle)
        {
            Initialize += OnInitialize;
        }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsoleWithSource(PPLogLevel.Log, "GettingStarted.HelloWorld", "HelloWorld from C#");
        }
    }
}
