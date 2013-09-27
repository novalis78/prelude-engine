using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using NLog;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace BotMemoryConverter
{
    class Program
    {
        private static Hashtable botsMemory = new Hashtable();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                //special tool
                //ConvertUserChatIntoDB();
                CleanMind();
                Console.ReadLine();


                //load mind text file
                StringCollection sc = new StringCollection();
                botsMemory.Clear();
                sc = readBrainFile("mind.mdu");
                StringEnumerator ii = sc.GetEnumerator();
                while (ii.MoveNext())
                {
                    if (!botsMemory.Contains(parseForThoughts(ii.Current)))
                        botsMemory.Add(parseForThoughts(ii.Current), parseForWords(ii.Current));
                }
                logger.Trace("Number of memo entries " + botsMemory.Count.ToString());

                //load mind-sqllite file
                int counter = 0;
                foreach (DictionaryEntry p in botsMemory)
                {
                    counter++;
                    string sql = "insert into memory (user, bot) VALUES ('" + MySqlEscape(p.Value.ToString()) + "','" + MySqlEscape(p.Key.ToString()) + "')";
                    Console.Write("\rConverting..." + counter);
                    ExecuteNonQuery(sql);
                }
                logger.Trace("Done converting...thanks. Have fun with Prelude's more advanced engine versions...");
                Console.ReadLine();
                //import mind into sqllite
            }
            catch (System.Exception ex)
            {
                logger.Trace("Error during converting operation: " + ex.Message);
            }
        }

        private static void ConvertUserChatIntoDB()
        {
            StringCollection sc = new StringCollection();
            botsMemory.Clear();
            sc = readBrainFile("chatlog.txt");
            StringEnumerator ii = sc.GetEnumerator();
            string user = "";
            string bot = "";
            
            while (ii.MoveNext())
            {
                if (ii.Current.StartsWith("surfingcurly820: "))
                    user += ii.Current.Substring(17);
                else if (ii.Current.StartsWith("Lisaalix91: "))
                    bot += ii.Current.Substring(11);

                if(!String.IsNullOrEmpty(user) && !String.IsNullOrEmpty(bot))
                {
                    if(!botsMemory.Contains(bot))
                        botsMemory.Add(bot, user);
                    user = "";
                    bot = "";
                }
            }
            prepareCurrentMemoryForDisc();
            Console.WriteLine("Done!");
            Console.ReadLine();

        }


        private static void CleanMind()
        {
            StringCollection sc = new StringCollection();
            botsMemory.Clear();
            sc = readBrainFile("mindtoclean.mdu");

            StringEnumerator ii = sc.GetEnumerator();
            while (ii.MoveNext())
            {
                if (ii.Current.ToLower().Contains("hint:") || ii.Current.ToLower().Contains("times up"))
                    continue;

                if (!botsMemory.Contains(parseForThoughts(ii.Current)))
                    botsMemory.Add(parseForThoughts(ii.Current), parseForWords(ii.Current));
            }
            logger.Trace("Number of memo entries " + botsMemory.Count.ToString());

            prepareCurrentMemoryForDisc();
            Console.WriteLine("Done!");
            Console.ReadLine();

        }

        public static void prepareCurrentMemoryForDisc()
        {
            StringCollection a = joinWordsAndThoughts();
            writeBrainFile(a);
        }

        private static StringCollection joinWordsAndThoughts()
        {
            StringCollection sc = new StringCollection();
            IDictionaryEnumerator de = botsMemory.GetEnumerator();
            while (de.MoveNext())
            {
                sc.Add("<USER>" + de.Value.ToString() + "</USER>" + "<BOT>" + de.Key.ToString() + "</BOT>");
            }
            return sc;
        }

        private static string parseForWords(string a)
        {
            string b = "";
            string tag = "USER";
            b = parseXMLContent(a, tag);
            return b;
        }

        private static string parseForThoughts(string a)
        {
            string b = "";
            string tag = "BOT";
            b = parseXMLContent(a, tag);
            return b;
        }

        private static string parseXMLContent(string xmlString, string xmlTag)
        {
            string content = "";
            string startTag = "<" + xmlTag + ">";
            string endTag = "</" + xmlTag + ">";
            int startPos = xmlString.IndexOf(startTag);
            int endPos = xmlString.IndexOf(endTag);
            if (startPos != -1 && endPos != -1)
            {
                startPos = startPos + startTag.Length;
                content = xmlString.Substring(startPos, (endPos - startPos));
            }
            return content;
        }


        private static StringCollection readBrainFile(string openFrom)
        {
            try
            {
                StringCollection sc = new StringCollection();
                if (File.Exists(openFrom))
                {
                    FileStream fs = new FileStream(openFrom, FileMode.Open, FileAccess.Read);
                    StreamReader rs = new StreamReader(fs);
                    string line;
                    while ((line = rs.ReadLine()) != null)
                    {
                        sc.Add(line);
                    }
                    rs.Close();
                    fs.Close();
                }
                return sc;
            }
            catch (System.Exception ex)
            {
                logger.Trace("Error loading mind file: " + ex.Message);
                return null;
            }
        }

        public static int ExecuteNonQuery(string sql)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            SQLiteConnection cnn = new SQLiteConnection("Data Source=" + startupPath + "\\mind.s3db");
            cnn.Open();
            SQLiteCommand mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            int rowsUpdated = mycommand.ExecuteNonQuery();
            cnn.Close();
            return rowsUpdated;

        }

        private static string MySqlEscape(string usString)
        {
            if (String.IsNullOrEmpty(usString))
            {
                return "";
            }
            // SQL Encoding for MySQL Recommended here:
            // http://au.php.net/manual/en/function.mysql-real-escape-string.php
            // it escapes \r, \n, \x00, \x1a, baskslash, single quotes, and double quotes
            usString = usString.Replace("\"", "&quot;");
            usString = usString.Replace(@"“", "&quot;");
            usString = usString.Replace(@"”", "&quot;");
            usString = usString.Replace(@"'", "&#39;");
            usString = usString.Replace("\'", "&#39;");
            usString = usString.Replace(@"’", "&#39;");
            usString = usString.Replace(@"′", "&#8217;");
            usString = usString.Replace(@"‘", "&#39;");
            return Regex.Replace(usString, @"[\r\n\x00\x1a\\'""]", @"\$0");
        }

        private static void writeBrainFile(StringCollection memoryBuffer)
        {
            string fullPathName = "chattomind.mdu";
            FileStream fs = new FileStream(fullPathName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            System.Collections.Specialized.StringEnumerator ii = memoryBuffer.GetEnumerator();
            while (ii.MoveNext())
            {
                string dat = ii.Current;
                sw.WriteLine(dat);
            }
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
