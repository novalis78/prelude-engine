using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreludeAddons
{
    public interface IPlugin
    {
        string returnBestAnswer(string userInput);
    }
}
