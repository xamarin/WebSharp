using System;

using PepperSharp;

namespace <%= className %>
{
    public class <%= className %> : Instance
    {
        public <%= className %> (IntPtr handle) : base(handle)
        {
            Initialize += OnInitialize;
        }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsoleWithSource(PPLogLevel.Log, "<%= className %>.<%= className %>", "Hello from C#");
        }
    }
}
