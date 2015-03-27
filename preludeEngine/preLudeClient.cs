/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 21.11.2004
 * Time: 01:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Threading;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Specialized;


namespace PreludeEngine
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	/// 
	public class PreLudeClient
	{
		public static string outputString = "";
		//the needed member fields
	    private TcpClient tcpc;
	    private string name= System.Environment.MachineName ;
	    //private static Hashtable botsMemory = null;
	    private string hostname = System.Environment.UserDomainName;
		//private string localIP  = System
	    public static int port=4244;
        //preludine.ath.cx
        public static string server = "preludine.ath.cx";
		
		public PreLudeClient()
		{
		}
		
		public void connectToPreludeServer(Hashtable botsMemory)
		{
	      this.name = name ;
	      try
	      {
		      //connect to the "localhost" at the give port
		      //if you have some other server name then you can use that 
		      //instead of "localhost"
		      tcpc =new TcpClient(server,port) ;
		      //get a Network stream from the server
		      NetworkStream nts = tcpc.GetStream();
		      //if the stream is writiable then write to the server
		      if(nts.CanWrite)
		      {
		        string sender = "Hi Server I am <BOTNAME>"+name+ "</BOTNAME> from "+hostname+"<MIND>";
			  	Byte[] sends = System.Text.Encoding.ASCII.GetBytes(sender.ToCharArray());
			    nts.Write(sends,0,sends.Length) ;
			    //flush to stream 
			    nts.Flush() ;
			    IDictionaryEnumerator de = botsMemory.GetEnumerator();
		        while(de.MoveNext())
		        {
		           string a = "<USER>" + de.Value.ToString() + "</USER>" + "<BOT>" + de.Key.ToString() + "</BOT>";
		           Byte[] sendMemo = System.Text.Encoding.ASCII.GetBytes(a.ToCharArray());
		           nts.Write(sendMemo,0,sendMemo.Length) ;
				   //flush to stream 
				   nts.Flush() ;
		        }
		        string c = "</MIND></QUIT>";
		           Byte[] sendQuit = System.Text.Encoding.ASCII.GetBytes(c.ToCharArray());
		           nts.Write(sendQuit,0,sendQuit.Length) ;
				   //flush to stream 
				   nts.Flush() ;
		        
			  }
			
      	}
      	catch(Exception e)
      	{
      	  string a = e.Message;//MessageBox.Show("Could not Connect to server because "+e.ToString());
	    }
	   }
	}
}
