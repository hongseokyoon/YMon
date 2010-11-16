using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace YMonClient
{
	public class SystemMonitor
	{
		private static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
		private static PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");

		private List<Process> _watchedProcessList = new List<Process>();

		#region Properties
		/// <summary>
		/// Get CPU Usage as percentage.
		/// </summary>
		public static float CpuUsage
		{
			get { return cpuCounter.NextValue(); }
		}

		/// <summary>
		/// Get available memory size as Mbyte.
		/// </summary>
		public static float AvailableMemory
		{
			get { return memCounter.NextValue(); }
		}

		//public static string[] Processes
		//{
		//    get
		//    {
		//        Process[] processes = Process.GetProcesses();

		//        List<string> processNameList = new List<string>();
		//        foreach (Process process in processes)
		//        {
		//            processNameList.Add(String.Format("{0},{1},{2}", process.ProcessName, process.Id, process.StartInfo.UserName));
		//        }

		//        processNameList.Sort();

		//        return processNameList.ToArray();
		//    }
		//}
		#endregion

		//public bool AddWatchedProcessByID(int processID)
		//{
		//    try
		//    {
		//        _watchedProcessList.Add(Process.GetProcessById(processID));
		//    }
		//    catch
		//    {
		//        return false;
		//    }

		//    return true;
		//}

		//public Process[] ExitedProcess()
		//{
		//    List<Process> ret = new List<Process>();

		//    foreach (Process p in _watchedProcessList) if (p.HasExited) ret.Add(p);
		//    foreach (Process p in ret) _watchedProcessList.Remove(p);

		//    return ret.ToArray();
		//}
	}
}
