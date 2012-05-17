using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginTester
{
    class Program
    {
        static void Main(string[] args)
        {
            PreludeAddons.PreludeModule pm = new PreludeAddons.PreludeModule();
            string answer = pm.returnBestAnswer("how is the weather today prelude");
            Console.ReadLine();
        }
    }
}
