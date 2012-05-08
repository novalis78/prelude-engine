/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 17.11.2004
 * Time: 20:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 * 
 */

 //TODO: more keywords, random start when too long idle
 //TODO: update from server, algorithm visualizer
 	
using System;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace PreludeEngine
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	/// 
	public class Mind : Brain
	{
		public Mind(string fileName, bool fullPath) : base(fileName,fullPath){}

		private static string input 	 = "";
		private static string lastOutput = "";
		private static StringCollection bestMatchesList = new StringCollection();
		protected Hashtable botsMemory			   	= new Hashtable();
		private static Hashtable matchedMemoryValues   	= new Hashtable();
		private const  int MAX_NUMBER_OF_IDENT_ENTRIES 	= 5;
		private const  int MAX_MATCHES_ALLOWED    		= 5;
		public  int memorySize = 0;
		public  bool proactiveMode = false;
		
		#region memory loading operations
		public void analyzeShortTermMemory()
		{
			purifyBotsMind();			
			StringCollection sc = new StringCollection();
			botsMemory.Clear();
			if(fullPathName == "")
				sc = readBrainFile();
			else
				sc = readBrainFile(fullPathName);
			StringEnumerator ii = sc.GetEnumerator();
			while(ii.MoveNext())
			{
				if(!botsMemory.Contains(parseForThoughts(ii.Current)))
					botsMemory.Add(parseForThoughts(ii.Current), parseForWords(ii.Current));
			}
			writeToLogFile("Number of memo entries", botsMemory.Count.ToString());
			memorySize = botsMemory.Count;
			return;
		}
		
		private string parseForWords(string a)
		{
			string b = "";
			string tag = "USER";
			b = parseXMLContent(a, tag);
			return b;
		}
		
		private string parseForThoughts(string a)
		{
			string b = "";
			string tag = "BOT";
			b = parseXMLContent(a, tag);
			return b;
		}
		
		private string parseXMLContent(string xmlString, string xmlTag)
		{
			string content = "";
			string startTag = "<" + xmlTag + ">";
			string endTag   = "</" + xmlTag + ">";
			int startPos = xmlString.IndexOf(startTag);
			int endPos   = xmlString.IndexOf(endTag);
			if(startPos != -1 && endPos != -1)
			{
				startPos = startPos + startTag.Length;
				content = xmlString.Substring(startPos, (endPos - startPos));
			}
			return content;
		}
		#endregion
		
		#region memory saving operations
		private void addNewInputToCurrentMemory(string input)
		{
			//user's input becomes possible output!
			//bot's last output becomes possible input!
			if(lastOutput != "")
			{
				if(!botsMemory.Contains(input))
					botsMemory.Add(input, lastOutput);
				writeToLogFile("", "-----------------------");
				writeToLogFile("LAST OUTPUT", lastOutput);				
				writeToLogFile("NEW INPUT", input);
			}
		}
		
		public void prepareCurrentMemoryForDisc()
		{
			StringCollection a = joinWordsAndThoughts();
			writeBrainFile(a);
		}
		public void prepareCurrentMemoryForDisc(string saveAs)
		{
			StringCollection a = joinWordsAndThoughts();
			writeBrainFile(a, saveAs);
		}		
		
		private StringCollection joinWordsAndThoughts()
		{
			StringCollection sc = new StringCollection();
			IDictionaryEnumerator de = botsMemory.GetEnumerator();
			while(de.MoveNext())
			{
				sc.Add("<USER>" + de.Value.ToString() + "</USER>" + "<BOT>" + de.Key.ToString() + "</BOT>");
			}
			return sc;
		}
		#endregion
		
		//gets input returns output
		public string listenToInput(string receivedInput)
		{
			input = receivedInput;	
			string output = "";
			addNewInputToCurrentMemory(input);
			checkInputForHiddenCommands(input);
			output = thinkItOver(input);
			lastOutput = output;
			return output;
		}
		
		#region thinking process
		//starts thinking process
		private string thinkItOver(string a)
		{
			string b = "";
			matchInputWithMemory(a);
			findBestMatchWithinMemory();	
			b = randomSelectAnswer(bestMatchesList);
			//dont allow bot to repeate its last sentence
			if(b == lastOutput) b = a;
			//bot echoes if it has no proper answer
			if(bestMatchesList.Count <= 0) b = a;
			return b;
		}
		//returns position of best match for input in memory
		private void matchInputWithMemory(string a)
		{
			int matchRate = 0;
			matchedMemoryValues.Clear();
			ArrayList inputSentenceTokenized = tokenizeString(a);
			IDictionaryEnumerator de = botsMemory.GetEnumerator();
			//run through memory
			while(de.MoveNext())
			{
				ArrayList t = tokenizeString((string)de.Value);
				matchRate   = calculateMatchRate(inputSentenceTokenized, t);
				if(matchRate != 0) matchedMemoryValues.Add(de.Key, matchRate);
			}
			return;
		}
		
		private void findBestMatchWithinMemory()
		{
			int i = 0;
			int highestValue = 0;
			bestMatchesList.Clear();
			if(matchedMemoryValues.Count > 0)
			{		
				IDictionaryEnumerator de = matchedMemoryValues.GetEnumerator();
				while(de.MoveNext())
				{
					if(highestValue <= (int)de.Value)
					{
						highestValue = (int)de.Value;
					}
				}
				//jetzt kennen wir den hchsten Wert,
				//zeit, die entsprechend hchstenwertigen stze rauszufischen:
				for(i = highestValue; i > 0; i--)
				{
					IDictionaryEnumerator re = matchedMemoryValues.GetEnumerator();
					while(re.MoveNext())
					{
						if(i == (int)re.Value)
							bestMatchesList.Add((string)re.Key);
						if(bestMatchesList.Count > MAX_MATCHES_ALLOWED)
							break;						
					}					
				}	
			}
		}
		
		//select randomly one sentence from preselected list of 
		//best responses
		private string randomSelectAnswer(StringCollection a)
		{
			string b = "";
			int z    = 0; 
			if(a.Count <= 0)
				return b;
			else
			{
				Random r = new Random();
				z = r.Next(0, a.Count);
				b = a[z];
				return b;
			}
		}
			
		#endregion
		
		
		#region mind purifying methods
		private void purifyBotsMind()
		{
			//killStickingPhrasesFromMemory();
			return;
		}
		//if bot sticks to a phrase simply limit its influence 
		//below a threshold number of occurences
		private void killStickingPhrasesFromMemory()
		{
			/*for(int r = 0; r < 2; r++)
			{
				int i = 0;
				string s = "";
				StringEnumerator se = words.GetEnumerator();
				while(se.MoveNext())
				{
					s = (string) se.Current;
					foreach(string o in words)
					{
						if( s == o) i++;
					}
					if(i > MAX_NUMBER_OF_IDENT_ENTRIES)
					{		//can delete only one word, count changes because of 
							//deletion
							writeToLogFile("deleting entry", s);
							writeToLogFile("because found it too often: ", i.ToString());
							int z = words.IndexOf(s);
							words.RemoveAt(z); 
							thoughts.RemoveAt(z);
							break;
					}
				}
			}*/
		}
		
		
		#endregion
		
		/*
		public static int MemorySize {
			get {
				return memorySize;
			}
			set {
				memorySize = value;
			}
		}
		*/
		#region utility methods
		private void checkInputForHiddenCommands(string a)
		{
			if((a.ToLower()).IndexOf("google") != -1)
			{
				string b = a.Substring(a.IndexOf("google") + 6);
				System.Diagnostics.Process.Start("IExplore", b);
			}
			if((a.ToLower()).IndexOf("open") != -1)
			{
				if(a.ToLower().IndexOf("word") != -1)
				{
					System.Diagnostics.Process.Start("winword");
				}
				if(a.IndexOf("notepad") != -1)
				{
					System.Diagnostics.Process.Start("notepad");
				}   
			}
			if((a.ToLower()).IndexOf("what") != -1)
			{
				if(a.ToLower().IndexOf("time") != -1)
				{
					System.Diagnostics.Process.Start("timedate.cpl");		
				}
			}
			if(a.ToLower().IndexOf("network") != -1)
			{
				System.Diagnostics.Process.Start("ncpa.cpl");			
			}
		}
		
		public void contributeClientMind()
		{
			PreLudeClient pc = new PreLudeClient();
			pc.connectToPreludeServer(botsMemory);
		}
		#endregion
		
	}
}
