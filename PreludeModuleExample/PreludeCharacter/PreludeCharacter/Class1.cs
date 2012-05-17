using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PreludeAddons
{
    public class PreludeModule : IPlugin
    {
        /// <summary>
        /// This method is called by the Prelude Engine when it detects plugins
        /// in its startup folder. It will add this answer to it's bot memory
        /// and thus make it possible for Prelude to add domain specific answers.
        /// 
        /// When you create your own module, make sure you keep the exact name and
        /// method definition. You can build the answer in any way you like
        /// 
        /// This example module uses a ip based geolocation service which tracks a
        /// user's weather and returns the answer to prelude when it detects the
        /// keywords "how" and "wheather". 
        /// </summary>
        /// <param name="userInput">the user's sentence</param>
        /// <returns>this module's best answer</returns>
        public string returnBestAnswer(string userInput)
        {
            string answer = "";
            userInput = userInput.ToLower();
            if (userInput.Contains("what") && userInput.Contains("your name"))
            {
                string[] strStrings = { "My name is Prelude", "Prelude, of course.", "why, don't you remember? It's Prelude" };
                // Choose a random slogan
                Random RandString = new Random();
                // Display the random slogan
                answer = strStrings[RandString.Next(0, strStrings.Length)];
            }
            return answer;
        }


        #region helper methods
        /// <summary>
        /// Retrieving an html document by downloading a website
        /// and casting it to an HTMLDocument object using HTMLAgility library
        /// </summary>
        /// <param name="link">the destination URL</param>
        /// <returns>The loaded HTML Document object</returns>
        

        #endregion
    }
}
