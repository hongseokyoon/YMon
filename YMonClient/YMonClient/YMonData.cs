using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace YMonClient
{
	class _Status
	{
		private int         _cpu;
		private int         _mem;
		private DateTime    _time;

		public int CPUUsage
		{
			get	{ return _cpu; }
		}

		public int AvailableMemory
		{
			get { return _mem; }
		}

		public DateTime Time
		{
			get { return _time; }
		}

		public _Status(int cpu, int mem, DateTime time)
		{
			_cpu    = cpu;
			_mem    = mem;
			_time   = time;
		}
	}

	class YMonData
	{
		private List<_Status> _statuses = new List<_Status>();

		#region Properties
        //public string Xml
        //{
        //    get { return _Xml2Str(_ToXml()); }
        //}
		#endregion

		public void AppendStatus(int cpu, int mem, DateTime time)
		{
			_statuses.Add(new _Status(cpu, mem, time));
		}

		public void ClearStatus()
		{
			_statuses.Clear();
		}

        public override string ToString()
        {
            return _Xml2Str(_ToXml());
        }

		private XmlDocument _ToXml()
		{
			XmlDocument doc = new XmlDocument();

			doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

			XmlElement  rootElement = doc.CreateElement("ymon");
			doc.AppendChild(rootElement);

			// statuses
			foreach (_Status status in _statuses)
			{
				XmlElement  statusElement   = doc.CreateElement("status");

				XmlElement  cpuElement      = doc.CreateElement("cpu");
				cpuElement.InnerText        = Convert.ToInt32(status.CPUUsage).ToString();

				XmlElement  memElement      = doc.CreateElement("mem");
				memElement.InnerText        = Convert.ToInt32(status.AvailableMemory).ToString();

				XmlElement  timeElement     = doc.CreateElement("time");
				timeElement.InnerText       = status.Time.ToLocalTime().ToString();

				statusElement.AppendChild(cpuElement);
				statusElement.AppendChild(memElement);
				statusElement.AppendChild(timeElement);

				rootElement.AppendChild(statusElement);
			}

			return doc;
		}

		private String _Xml2Str(XmlDocument doc)
		{
			System.IO.StringWriter  sw      = new System.IO.StringWriter();
			XmlTextWriter           xmltw   = new XmlTextWriter(sw);
			xmltw.Formatting                = Formatting.Indented;
			doc.WriteTo(xmltw);

			return sw.ToString();
		}
	}
}
