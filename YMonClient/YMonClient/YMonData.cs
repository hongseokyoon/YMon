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
        
        public XmlElement ToXmlElement(XmlDocument doc)
        {
            XmlElement  statusElement   = doc.CreateElement("status");

			XmlElement  cpuElement      = doc.CreateElement("cpu");
			cpuElement.InnerText        = Convert.ToInt32(CPUUsage).ToString();

			XmlElement  memElement      = doc.CreateElement("mem");
			memElement.InnerText        = Convert.ToInt32(AvailableMemory).ToString();

			XmlElement  timeElement     = doc.CreateElement("time");
			timeElement.InnerText       = String.Format("{0}/{1}/{2} {3}:{4}:{5}.{6}", 
                _time.Year, _time.Month, _time.Day, _time.Hour, _time.Minute, _time.Second, _time.Millisecond);

			statusElement.AppendChild(cpuElement);
			statusElement.AppendChild(memElement);
			statusElement.AppendChild(timeElement);

            return statusElement;
        }

		public _Status(int cpu, int mem, DateTime time)
		{
			_cpu    = cpu;
			_mem    = mem;
			_time   = time.ToUniversalTime();
		}
	}

    class _File
    {
        private string  _name;
        private string  _desciption;
        private string  _data;

        public _File(string path, string name, string description)
        {
            _data       = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
            _name       = name;
            _desciption = description;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _desciption; }
        }

        public string Data
        {
            get { return _data; }
        }

        public XmlElement ToXmlElement(XmlDocument doc)
        {
            XmlElement  fileElement = doc.CreateElement("file");

            XmlElement  nameElement = doc.CreateElement("name");
            nameElement.InnerText   = Name;

            XmlElement  descriptionElement  = doc.CreateElement("description");
            descriptionElement.InnerText    = Description;

            XmlElement  dataElement         = doc.CreateElement("data");
            dataElement.InnerText           = Data;

            fileElement.AppendChild(nameElement);
            fileElement.AppendChild(descriptionElement);
            fileElement.AppendChild(dataElement);

            return fileElement;
        }
    }

    class _Dump
    {
        private string      _description;
        private DateTime    _time;
        private List<_File> _files  = new List<_File>();

        public _Dump(string description, DateTime time)
        {
            _description    = description;
            _time           = time.ToUniversalTime();
        }

        public string Description
        {
            get { return _description; }
        }

        public DateTime Time
        {
            get { return _time; }
        }

        public List<_File> Files
        {
            get { return _files; }
        }

        public XmlElement ToXmlElement(XmlDocument doc)
        {
            XmlElement  dumpElement = doc.CreateElement("dump");

            XmlElement  descriptionElement  = doc.CreateElement("description");
            descriptionElement.InnerText    = Description;

            XmlElement  timeElement         = doc.CreateElement("time");
            timeElement.InnerText           = String.Format("{0}/{1}/{2} {3}:{4}:{5}.{6}", 
                _time.Year, _time.Month, _time.Day, _time.Hour, _time.Minute, _time.Second, _time.Millisecond);

            dumpElement.AppendChild(descriptionElement);
            dumpElement.AppendChild(timeElement);

            foreach (_File file in _files)
            {
                dumpElement.AppendChild(file.ToXmlElement(doc));
            }

            return dumpElement;
        }

        public bool AppendFile(string path)
        {
            return AppendFile(path, null, null);
        }

        public bool AppendFile(string path, string name)
        {
            return AppendFile(path, name, null);
        }

        public bool AppendFile(string path, string name, string description)
        {
            if (!System.IO.File.Exists(path))   return false;

            if (name == null)
            {
                name    = (new System.IO.FileInfo(path)).Name;
            }

            _files.Add(new _File(path, name, description));

            return true;
        }
    }

	class YMonData
	{
		private List<_Status>   _statuses   = new List<_Status>();
        private List<_Dump>     _dumps      = new List<_Dump>();

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
				rootElement.AppendChild(status.ToXmlElement(doc));
			}

            foreach (_Dump dump in _dumps)
            {
                rootElement.AppendChild(dump.ToXmlElement(doc));
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
