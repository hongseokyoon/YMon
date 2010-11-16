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
		System.IO.TextWriter _tw = null;

		public Service()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			_tw = new System.IO.StreamWriter("c:\\ymon.txt");
			timer.Start();
		}

		protected override void OnStop()
		{
			timer.Stop();
			_tw.Close();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (_tw != null)
			{
				_tw.WriteLine(String.Format("YMon[{0}] CPU Usage {1} % Available Memory {2} MB",
					System.DateTime.Now.ToLocalTime().ToString(), SystemMonitor.CpuUsage, SystemMonitor.AvailableMemory));
				_tw.Flush();
			}
		}
	}
}
