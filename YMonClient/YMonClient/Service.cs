using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace YMonClient
{
	public partial class Service : ServiceBase
	{
		System.IO.StreamWriter _sw	= null;//new System.IO.StreamWriter("c:\\ymon.txt");

		public Service()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			_sw = new System.IO.StreamWriter("c:\\ymon.txt");
		}

		protected override void OnStop()
		{
			_sw.Close();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			_sw.WriteLine(String.Format("YMon[{0}] CPU Usage {1} % Available Memory {2} MB",
				System.DateTime.Now.ToLocalTime().ToString(), SystemMonitor.CpuUsage, SystemMonitor.AvailableMemory));
			_sw.Flush();
		}
	}
}
