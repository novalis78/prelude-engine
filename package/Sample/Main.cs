/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 05.12.2004
 * Time: 15:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using PreLudeEngine;

namespace pleTest
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("going to test PLE");
			//initialize interface
			PreLudeInterface pi = new PreLudeInterface();
			//define path to mind file
			pi.loadedMind = "mind.mdu";			
			//start your engine ...
			pi.initializeEngine();
			//here we go:
			Console.WriteLine("You say: " + "hello Prelude");
			Console.WriteLine("Prelude says: " + pi.chatWithPrelude("hello Prelude"));
			pi.stopPreludeEngine();
		}
	}
}
