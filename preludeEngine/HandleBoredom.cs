using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreludeEngine
{
    public class HandleBoredom : EventArgs
    {
        private string EventInfo;
        public HandleBoredom(string Text)
        {
                EventInfo = Text;
        }
        public string GetInfo()
        {
                return EventInfo;
        }
    }
}
