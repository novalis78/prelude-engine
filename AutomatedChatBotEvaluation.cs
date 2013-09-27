///
///
/// Copyright (C) 2006  Lennart Lopin <lennart [dot] lopin [at] gmail [dot] com> 
/// Thanks go to "Cyberwolf" <cyberwolf 2000 [at] gmx [dot] net> for some very valuable ideas and code improvements
/// All Rights Reserved.
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License as
/// published by the Free Software Foundation; either version 2 of the
/// License, or (at your option) any later version.
/// 
/// This program is distributed in the hope that it will be useful, but
/// WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
/// General Public License for more details.
/// 
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
/// 02111-1307, USA.
/// 
///

using System;
using PreludeEngine;
using System.Collections;

namespace PreludeEngineQA
{
	/// <summary>
	/// Automated evaluation of a (self-learning) chat bot's response quality
	/// </summary>
	class Class1
	{
		private static ArrayList testInput = null;
		private static Hashtable expectedA = null;
		private static PreLudeInterface pi = null;


		[STAThread]
		static void Main(string[] args)
		{
			
			//Initialize your Chatbot (dialog engine)
			//replace the reference "pi" with your own chatbots counterpart
			pi = new PreLudeInterface();
			pi.loadedMind = "test.mdu";
			pi.initializeEngine();

			//test suite starts here

			
			testInput = AddNumberOfInputSentences();
			//
			expectedA = createWishList();
			float engineQuality = 0.0F;

			for(int i = 0 ; i < 5; i++)
			{
				engineQuality = repeat(testInput, 100);
				Console.WriteLine("I tested your current chat bot engine ");
				Console.WriteLine("On my chat intelligence scale it acquires: " + engineQuality + " points. Congrats");
				Console.WriteLine("That means, from a possible value of 6 you archieved: ");
				Console.WriteLine(engineQuality + " which is " +  (engineQuality / 0.06F) + "%");
			}
			Console.ReadLine();
		}

		//
		//this is the number of 10 "new" (i.e. not known to the chatbot) sentences
		//
		private static ArrayList AddNumberOfInputSentences()
		{
			ArrayList al = new ArrayList();
			al.Add("Hello, how are you");
			al.Add("What did you eat today?");
			al.Add("What is your name?");
			al.Add("Shall we meet one day?");
			al.Add("Let me put this straight");
			al.Add("I like hotels");
			al.Add("I dont want to chat with you any longer");
			al.Add("Who is your father");
			al.Add("Did you ever have sex?");
			al.Add("You look lovely today");
			return al;
		}

		//
		//this is where the evaluation from the chatbots response compared to the input and test case
		//happens
		//
		private static float evaluate(ArrayList al)
		{
			float singleDialogQuality = 0.0F;

			IEnumerator ie = al.GetEnumerator();
			while(ie.MoveNext())
			{
				//
				//The chatbots answer to each of the newly presented sentences
				//is being evaluated according to the set of given test sentences
				//
				string answer = pi.chatWithPrelude((string)ie.Current);
				singleDialogQuality += getValueForCurrentPair(answer, (string)ie.Current);
			}
			return singleDialogQuality / al.Count;
		}

		private static float repeat(ArrayList al, int times)
		{
			float engineQualityValue = 0.0F;

			for(int i = 0; i < times; i++)
			{
				engineQualityValue += evaluate(al); 
			}

			return engineQualityValue / times;
		}

		private static float getValueForCurrentPair(string answer, string input)
		{
			float mvalue = 0.0F;
			if(expectedA.ContainsKey(input))
			{
				string[] possAnswers = (string[])expectedA[input];
				for(int i = 0; i < possAnswers.Length; i++)
				{
					if(answer.Equals(possAnswers[i]))
					{
						mvalue = (i == 0) ? 6.0F : ((i == 1) ? 4.0F : 2.0F);
					}
				}
				return mvalue;
					
			}
			else
			{
				Console.WriteLine("Error: an input string is not reflected in the possible 'expected answers list'");
				return 0.0F;
			}
		}

		//
		//So, this is what we would like to see from our chatbot in a "perfect" world. 
		//For each "new" input (key) we "design" three possible answers (value) which our
		//chatbot can chose from. He gets points (6,4,2,0) for his selection...
		//
		private static Hashtable createWishList()
		{
			Hashtable ht = new Hashtable();
			ht.Add("Hello, how are you", new string[]{"I am fine", "Good", "Fine, how are you doing?"});
			ht.Add("What did you eat today?", new string[]{ "I am starving. Had no dinner yet", "I ate fish and chips", "Nothing"});
			ht.Add("What is your name?", new string[]{"I am a chatbot without names", "My name is Prelude", "I have none"});
			ht.Add("Shall we meet one day?", new string[]{"Yes, i'd like to meet you", "i hope not so", "oh please"});
			ht.Add("Let me put this straight", new string[]{"Go on", "Continue", "You are welcome"});
			ht.Add("I like hotels", new string[]{"Me too", "I like them too", "Great"});
			ht.Add("I dont want to chat with you any longer", new string[]{"You dont have to. You may go.", "Bye Bye", "See you"});
			ht.Add("Who is your father", new string[]{"I dont have to tell you those details, have i?", "I cant remember his name", "Who"});
			ht.Add("Did you ever have sex?", new string[]{"Every day - with you", "Still learning the details", "No never"});
			ht.Add("You look lovely today", new string[]{"Thank you for the sweet compliment", "Nice remark, that", "Very good"});
			return ht;
		}

	}
}

///For the record, the "test.mdu" with the bots learnt memory:
///
/*
 * <USER>how are you doing today?</USER><BOT>I am fine</BOT>
<USER>how do you do today?</USER><BOT>Good</BOT>
<USER>hi my friend how are you?</USER><BOT>Fine, how are you doing</BOT>
<USER>did you eat dinner?</USER><BOT>I am starving. Had no dinner yet</BOT>
<USER>what do you like to eat?</USER><BOT>I ate fish and chips</BOT>
<USER>did you have lunch?</USER><BOT>Nothing</BOT>
<USER>what is your cool name?</USER><BOT>I am a chatbot without names</BOT>
<USER>what about your name</USER><BOT>My name is Prelude</BOT>
<USER>do you have such a thing like a name</USER><BOT>I have none</BOT>
<USER>want to meet me in real life</USER><BOT>Yes, i'd like to meet you</BOT>
<USER>do you like to join me</USER><BOT>i hope not so</BOT>
<USER>let us go to a party you and me</USER><BOT>oh please</BOT>
<USER>listen to this</USER><BOT>Go on</BOT>
<USER>you better hear this out</USER><BOT>Continue</BOT>
<USER>better follow my advise</USER><BOT>You are welcome</BOT>
<USER>what do you think about it? i enjoy it.</USER><BOT>Me too</BOT>
<USER>ever stayed there? you like' em</USER><BOT>I like them too</BOT>
<USER>how do you find those hotels</USER><BOT>Great</BOT>
<USER>do i need to stay here</USER><BOT>You dont have to. You may go.</BOT>
<USER>see you my friend</USER><BOT>Bye Bye</BOT>
<USER>bye bye till later</USER><BOT>See you</BOT>
<USER>i want to know everything about you</USER><BOT>I dont have to tell you those details, have i?</BOT>
<USER>who was your best friend</USER><BOT>I cant remember his name</BOT>
<USER>tell me more about him</USER><BOT>Who</BOT>
<USER>do you have an intimate relationship wih someone?</USER><BOT>Every day - with you</BOT>
<USER>do you know anything about it?</USER><BOT>Still learning the details</BOT>
<USER>you smoke</USER><BOT>No never</BOT>
<USER>i like your little nose</USER><BOT>Thank you for the sweet compliment</BOT>
<USER>your hair is lovely</USER><BOT>Nice remark, that</BOT>
<USER>your skin is so soft</USER><BOT>Very good</BOT>
<USER>how should i call you</USER><BOT>ZZZ</BOT>
*/
///
