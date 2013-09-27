/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 17.11.2004
 * Time: 20:16
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace PreludeEngine
{
	/// <summary>
	/// Description of Brain
	/// </summary>
	public class Brain : SynapsesBase
	{
		
		private static string 	filePath = "";
		private static string	currDir  = "";
		protected static string   fullPathName = "";
		
		#region brain constructor

		public Brain(string fileName, bool fullPath)
		{
			currDir  = Directory.GetCurrentDirectory();
			filePath = currDir + "\\" + fileName;
			if(fullPath)
			{
				readBrainFile(fileName);
				fullPathName = fileName;
			}
			else
				readBrainFile();
		}
		#endregion
		
		#region write/read brain files
		//read brain file from disc
		protected StringCollection readBrainFile()
		{
			StringCollection sc = new StringCollection();
            if (filePath.Contains("s3db"))
            {
                if (File.Exists(filePath))
                { 
                    
                }
            }
            else if(File.Exists(filePath))
	    	{
	    	   	FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
	    	   	StreamReader rs = new StreamReader(fs);
	    	   	string line;
	    	   	while ((line = rs.ReadLine()) != null) 
                {
	    	   		sc.Add(line);
	    	   	}	    	   	
	    	   	rs.Close();
	    	   	fs.Close();
	    	}
	    	else
	    	{
	    		MessageBox.Show("No mind file found, creating new one " + Directory.GetCurrentDirectory());
	    		FileStream cs = File.Create(filePath);
	    		cs.Close();
	    		FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
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

		protected StringCollection readBrainFile(string openFrom)
		{
			StringCollection sc = new StringCollection();
	    	if(File.Exists(openFrom))
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
		//write brain file to disc
		protected void writeBrainFile(StringCollection memoryBuffer)
		{
			if(fullPathName != "")
			{
				writeBrainFile(memoryBuffer, fullPathName);
				return;
			}
			File.Delete(filePath);
	    	FileStream fs = new FileStream(filePath , FileMode.OpenOrCreate, FileAccess.Write); 
            StreamWriter sw = new StreamWriter(fs); 
			sw.BaseStream.Seek(0, SeekOrigin.End); 
			System.Collections.Specialized.StringEnumerator ii = memoryBuffer.GetEnumerator();
			while(ii.MoveNext())
			{
				string dat = ii.Current;
				sw.WriteLine(dat);
			}	
			sw.Flush();
			sw.Close();
			fs.Close();	
		}
		//for saving as with custom location
		protected void writeBrainFile(StringCollection memoryBuffer, string saveAs)
		{
	    	/*File.Delete(saveAs);
	    	FileStream fs = new FileStream(saveAs , FileMode.OpenOrCreate, FileAccess.Write); 
            StreamWriter sw = new StreamWriter(fs); 
			sw.BaseStream.Seek(0, SeekOrigin.End); 
			System.Collections.Specialized.StringEnumerator ii = memoryBuffer.GetEnumerator();
			while(ii.MoveNext())
			{
				string dat = ii.Current;
				sw.WriteLine(dat);
			}	
			sw.Flush();
			sw.Close();
			fs.Close();	*/
		}
		#endregion
		
		#region logging
		protected void writeToLogFile(string header, string message)
		{
			/*DateTime dtNow;
			dtNow = DateTime.Now;
			string logFile = currDir + "\\" + dtNow.ToShortDateString() + "_" + "log.txt";
	    	FileStream fs = new FileStream(logFile , FileMode.OpenOrCreate, FileAccess.Write); 
            StreamWriter sw = new StreamWriter(fs); 
            sw.BaseStream.Seek(0, SeekOrigin.End); 
            sw.WriteLine(dtNow.ToShortTimeString() + " [" + header + "] : " + message);
            sw.Flush();
			sw.Close();
			fs.Close();	*/
		}
		#endregion
	}
}
