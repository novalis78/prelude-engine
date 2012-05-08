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

namespace pleTest
{
	class MainClass
	{
        
		private static string ind = "";
		public static void Main(string[] args)
		{
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			Console.WriteLine("Prelude@# ("+version+") command line version, welcome user!");
			Console.WriteLine("if you want to stop chatting, enter: 'exit'");
			//initialize interface
			PreLudeInterface pi = new PreLudeInterface();
			//define path to mind file
			pi.loadedMind = "mind.mdu";			
			//start your engine ...
			pi.initializeEngine();
			//here we go:
			while(!ind.StartsWith("exit"))
			{
				Console.Write("You say: ");
				ind = Console.ReadLine();
				Console.WriteLine("Prelude says: " + pi.chatWithPrelude(ind));
			}
			pi.stopPreludeEngine();
		}
	}
}
