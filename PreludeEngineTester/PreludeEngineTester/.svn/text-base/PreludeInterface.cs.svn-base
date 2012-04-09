/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 05.12.2004
 * Time: 11:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using SpeechLib;
using System.Threading;
using System.Windows.Forms;
using System.Timers;

namespace PreludeEngine
{
	/// <summary>
	/// TODO: check speech engine -- if too fast it stops
	/// that never happens when using msagent speechlib
	/// 2.) incorporate speechrecognition
	/// </summary>
	public class PreLudeInterface
	{
		private Mind mindInstance = null;	
		public string loadedMind 	= "mind.mdu";
		public bool isContributable = false;
		public bool isSpeaking      = false;
		public bool proactiveMode   = true;
		private int idleTime 		= 0;
		private string autoSpeakInput 		= "";
		private System.Timers.Timer timer 	= null;
		public delegate string AutoSpeakHandler(string boredString);
		public AutoSpeakHandler reportBoredom;
		
		public void initializeEngine()
		{
			mindInstance = new Mind(loadedMind, false);
			mindInstance.analyzeShortTermMemory();
			if(proactiveMode)
			{
				timer = new System.Timers.Timer();
				timer.Elapsed += new System.Timers.ElapsedEventHandler(autoAnswering);
			}
		}
		
		public string chatWithPrelude(string input)
		{
			if(mindInstance == null) return "Error: Mind not initialized";
			if(proactiveMode)
			{
				idleTime = 0;
				if(timer != null)
					timer.Stop();
			}
			string output = "";
			output = mindInstance.listenToInput(input);
			if(isSpeaking)
				speak(output);
			if(proactiveMode)
			{
				Random random = new Random();
				idleTime = random.Next(15000, 30000); 
				timer.Interval = idleTime;
				autoSpeakInput = output;
				timer.Start();
			}
			return output;	
		}
		
		public void autoAnswering(object sender, System.Timers.ElapsedEventArgs e)
		{
			//trigger auto answer to frontend
			if(timer.Enabled != false)
			{
				reportBoredom(mindInstance.listenToInput(autoSpeakInput));
			}
				
		}
	
		
		public void speak(string a)
		{
			if(mindInstance == null) return;
			try
            {
            	SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
                SpVoice speech = new SpVoice();
                if (isSpeaking)
                {
                    speech.Speak(a, SpFlags);    
                }
        	}
        	catch
            {
            	MessageBox.Show("Speech Engine error");
            }
		}
		
		public void stopPreludeEngine()
		{
			if(mindInstance == null) return;
			mindInstance.prepareCurrentMemoryForDisc();
			if(isContributable)
			   mindInstance.contributeClientMind();
		}
		
		public void forcedContribution()
		{	
			if(mindInstance == null) return;
			if(isContributable)
			   mindInstance.contributeClientMind();
		}
		//save current mind to disc
		public void forcedSaveMindFile()
		{
			if(mindInstance == null) return;
			mindInstance.prepareCurrentMemoryForDisc();
		}
		
		//save current mind to disc there is another way too!
		public void forcedSaveMindFile(string a)
		{
			if(mindInstance == null) return;
			mindInstance.prepareCurrentMemoryForDisc(a);
		}		
		
		//count currently loaded bot memory
		public int countMindMemory()
		{
			if(mindInstance == null) return -1;
			int i = 0;
			i = mindInstance.memorySize;
			return i;
		}
		
		public string getVersionInfo()
		{
			return "Prelude@# Engine, version 0.4.0, 2004-05(c) by Lennart Lopin ";
		}
		
		public bool setPreludeClient(string server, int port)
		{
			if(mindInstance != null) return false;
			PreLudeClient.port = port;
			PreLudeClient.server = server;
			return true;
		}
		
		public bool setProactiveMode(bool a)
		{
			mindInstance.proactiveMode = a;
			return false;
		}
	}
}
