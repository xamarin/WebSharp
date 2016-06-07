using System;
using Pepper;

namespace HelloWorld
{
    public class HelloWorld : PPInstance
    {

        public HelloWorld(IntPtr handle) : base(handle) { }

        ~HelloWorld()
        {
            System.Console.WriteLine("HelloWorld destructed");
        }

        public override bool Init(int argc, string[] argn, string[] argv)
        {

            for (int x = 0; x < argc; x++)
            {
                Console.WriteLine($"property \"{argn[x]}\" = {argv[x]}");
            }
            return true;
        }

    }
}
