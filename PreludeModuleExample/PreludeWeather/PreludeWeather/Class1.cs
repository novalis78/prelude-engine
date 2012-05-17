using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreludeWeather
{
    public class PreludeWeather
    {
        /// <summary>
        /// This method is called by the Prelude Engine when it detects plugins
        /// in its startup folder. It will add this answer to it's bot memory
        /// and thus make it possible for Prelude to add domain specific answers
        /// </summary>
        /// <param name="userInput">the user's sentence</param>
        /// <returns>this module's best answer</returns>
        public string returnBestAnswer(string userInput)
        {
            string answer = "";
            userInput = userInput.ToLower();
            if (userInput.Contains("how") && userInput.Contains("weather"))
            { 
                
            }
            return answer;
        }
    }
}
