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
using AForge.Math.Metrics;
using System.Collections.Generic;
using NLog;

namespace PreludeEngine
{
	/// <summary>
	/// Description of StringBase.
	/// </summary>
	public class SynapsesBase
	{
        private static Logger logger = LogManager.GetCurrentClassLogger();

		public SynapsesBase()
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
                char[] delimiters = new char[] { ',', ':', '!', '?', ';', '-', ' ', '~', '^' };
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

        /// <summary>
        /// JaccardDistance calculation to determine 
        /// matching similar responses
        /// </summary>
        /// <param name="input"></param>
        /// <param name="memory"></param>
        /// <returns></returns>
        protected double calculateMatchRateJ(ArrayList input, ArrayList memory)
        {
            // instantiate new distance class
            JaccardDistance dist = new JaccardDistance();
            // create two vectors for inputs
            //double[] p = new double[] { 2.5, 3.5, 3.0, 3.5, 2.5, 3.0 };
            double[] p = DigitizePhonemes(input);
            //double[] q = new double[] { 3.0, 3.5, 1.5, 5.0, 3.5, 3.0 };
            double[] q = DigitizePhonemes(memory);

            List<double> q1 = EqualizeVector(p, q);

            double[] q2 = q1.ToArray();
            // get distance between the two vectors
            double distance = dist.GetDistance(p, q2);
            return distance;
        }

        /// <summary>
        /// Levenshtein distance calculation to determine 
        /// matching similar responses
        /// </summary>
        /// <param name="input">received sounds through ear sense</param>
        /// <param name="memory">stored and signed sounds in bot's memory</param>
        /// <returns></returns>
        protected double calculateMatchRateLS(ArrayList input, ArrayList memory)
        {
            // even though we did not need to separate the string 
            // for the Levensthein distance other algorithm implementations need
            // the word matrix... we will (for now) just join the words.
            string one = String.Join(" ", input.ToArray());
            string two = String.Join(" ", memory.ToArray());

            double distance = LD(one, two);
            return distance;
        }

        /// <summary>
        /// Dice Coefficient
        /// </summary>
        /// <param name="input">received sounds through ear sense</param>
        /// <param name="memory">stored and signed sounds in bot's memory</param>
        /// <returns></returns>
        protected double calculateMatchRateDice(ArrayList input, ArrayList memory)
        {
            // even though we did not need to separate the string 
            // for the Levensthein distance other algorithm implementations need
            // the word matrix... we will (for now) just join the words.
            string one = String.Join(" ", input.ToArray());
            string two = String.Join(" ", memory.ToArray());

            double distance = DiceCoefficient(one, two);
            return distance;
        }

        

        private static List<double> EqualizeVector(double[] p, double[] q)
        {
            List<double> q1 = new List<double>();
            if (q.Length > p.Length)
            {
                for (int i = 0; i < p.Length; i++)
                {
                    q1.Add(q[i]);
                }
            }
            else if (q.Length < p.Length)
            {
                for (int i = 0; i < q.Length; i++)
                {
                    q1.Add(q[i]);
                }
                int diff = p.Length - q.Length;
                for (int i = 0; i < diff; i++)
                {
                    q1.Add(0.0);
                }
            }
            else
            {
                for (int i = 0; i < q.Length; i++)
                {
                    q1.Add(q[i]);
                }
            }
            return q1;
        }

        private double[] DigitizePhonemes(ArrayList input)
        {
            List<Double> d = new List<Double>();
            foreach(String ph in input)
            {
                foreach (char c in ph)
                {
                    d.Add((double)c);
                }
            }
            return d.ToArray();
        }


        /// <summary>
        /// Compute Levenshtein distance
        /// </summary>
        /// <param name="s">String 1</param>
        /// <param name="t">String 2</param>
        /// <returns>Distance between the two strings.
        /// The larger the number, the bigger the difference.
        /// </returns>
        public double LD(string s, string t)
        {
            int n = s.Length; //length of s
            int m = t.Length; //length of t
            int[,] d = new int[n + 1, m + 1]; // matrix
            int cost; // cost
            if (n == 0) return m;
            if (m == 0) return n;
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1);
                    d[i, j] = System.Math.Min(System.Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                              d[i - 1, j - 1] + cost);
                }
            }
            int a = d[n, m];
            if (a > 0)
            {
                //logger.Trace("V: " + 1.0 / a);
                return 1.0 / a;
            }
            else
                return 0;
        }

        /// <summary>
        /// Dice's coefficient measures how similar a set and another set are. It can be used to measure how similar two strings are 
        /// in terms of the number of common bigrams (a bigram is a pair of adjacent letters in the string).
        /// </summary>
        /// <param name="stOne"></param>
        /// <param name="stTwo"></param>
        /// <returns></returns>
        public double DiceCoefficient(string stOne, string stTwo)
        {
            try
            {
                HashSet<string> nx = new HashSet<string>();
                HashSet<string> ny = new HashSet<string>();

                for (int i = 0; i < stOne.Length - 1; i++)
                {
                    char x1 = stOne[i];
                    char x2 = stOne[i + 1];
                    string temp = x1.ToString() + x2.ToString();
                    nx.Add(temp);
                }
                for (int j = 0; j < stTwo.Length - 1; j++)
                {
                    char y1 = stTwo[j];
                    char y2 = stTwo[j + 1];
                    string temp = y1.ToString() + y2.ToString();
                    ny.Add(temp);
                }

                HashSet<string> intersection = new HashSet<string>(nx);
                intersection.IntersectWith(ny);

                double dbOne = intersection.Count;
                double res = (2 * dbOne) / (nx.Count + ny.Count);
                //logger.Trace("V: " + res);
                return res;
            }
            catch (System.Exception ex)
            {
                logger.Trace("error: " + ex.Message);
                return 0;
            }

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
