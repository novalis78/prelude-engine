/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 17.11.2004
 * Time: 22:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace PreludeEngine
{
	/// <summary>
	/// Description of StringBase.
	/// </summary>
	public class StringBase
	{
		public StringBase()
		{
		}
		//break up string into words
		protected ArrayList tokenizeString(string a)
		{
            ArrayList al = new ArrayList();

            if (String.IsNullOrEmpty(a))
                return al;
            try
            {
                char[] delimiters = new char[] { ',', ':', '!', '?', ';', '-', ' ' };
                string[] parts = a.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach(string tok in parts)
                        al.Add(tok);  
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error in tokenizer: " + ex.Message);
            }
			return al;
		}
		//return how closely an input string resembles a single memory entry
		//break because dont want to count same word re-occurences
		protected int calculateMatchRate(ArrayList input, ArrayList memory)
		{
			int matchRate = 0;
			string cc     = "";
			string bb     = "";
			
			IEnumerator i = input.GetEnumerator();
			while(i.MoveNext())
			{
				cc = (string)i.Current;
				cc = cc.ToLower();
				
				IEnumerator m = memory.GetEnumerator();
				while(m.MoveNext())
				{
					bb = (string)m.Current;
					bb = bb.ToLower();
					if(cc == bb)
					{
						matchRate++; break;
					} 
				}
				
			}
			return matchRate;
		}
		
		protected ArrayList removeRedundantEntries(ArrayList a)
		{
			IEnumerator i = a.GetEnumerator();
			while(i.MoveNext())
			{
				;
			}
			return a;
		}
		
	}
}
