/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 05.12.2004
 * Time: 15:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using PreludeEngine;
using NLog;

namespace pleTest
{
	class MainClass
	{
        
		private static string ind = "";
        private static Logger logger = LogManager.GetCurrentClassLogger();

		public static void Main(string[] args)
		{
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			Console.WriteLine("Prelude@# ("+version+") command line version, welcome user!");
			Console.WriteLine("if you want to stop chatting, enter: 'exit'");
			//initialize interface
			PreLudeInterface pi = new PreLudeInterface();
			
            //configure prelude
            //define path to mind file
			pi.loadedMind = "mind.mdu";
            //decide whether you want true randomness
            pi.quantumRandomness = false;
            pi.isSpeaking = true;
            pi.setProactiveMode(true);
            pi.avoidLearnByRepeating = true;
            
            pi.initializedAssociater = Mind.MatchingAlgorithm.Dice;

			//start your engine ...
			pi.initializeEngine();
			//here we go:
			while(!ind.StartsWith("exit"))
			{
				Console.Write("You say: ");
				ind = Console.ReadLine();
                logger.Trace("You say: " + ind);
                string answer = pi.chatWithPrelude(ind);
				Console.WriteLine("Prelude says: " + answer);
                logger.Trace("Prelude says: " + answer);
			}
			pi.stopPreludeEngine();
		}
	}
}
