/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 20.04.2005
 * Time: 20:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Threading;
using System.Diagnostics;
using System.Timers;

namespace PreLudeEngine
{
	/// <summary>
	/// Description of AutoSpeaker.
	/// </summary>
	public class AutoSpeaker
	{
		public readonly Thread Thread;
		public bool terminationFlag = false;
		public delegate void ThreadCBDelegate(int status, string msg);
		public ThreadCBDelegate reportStatus;
		public int idleTime = 0;
		public System.Timers.Timer timer = null;
		
		public AutoSpeaker()
		{
			this.Thread = new Thread(new ThreadStart(countDownTimer));
		}
		
		public void countDownTimer()
		{
			Console.WriteLine("Hi ich bin der Thread");
			while(true)
			{			
				Thread.Sleep(idleTime);
				if(terminationFlag)
					continue;
				else if(idleTime != 0)
					reportStatus(idleTime, "sag was");
			}
		}
		
		public void createIdleTime()
		{
			Random random = new Random();
			idleTime = random.Next(15000, 30000); 
			timer = new System.Timers.Timer();
			timer.Interval = idleTime;
			timer.Start();
		}
		
		public void resetIdleTime()
		{
			idleTime = 0;
			timer.Stop();
		}
	}
}
