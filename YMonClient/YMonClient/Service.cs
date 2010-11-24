using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Net;

namespace YMonClient
{
	public partial class Service : ServiceBase
	{
		//System.IO.TextWriter    _tw     = null;
		System.Timers.Timer     _timer  = new System.Timers.Timer();
		YMonData                _data   = new YMonData();
        readonly int            _requestInterval    = 2;
        int                     _tickCounter        = 0;
        readonly String         _requestUri         = @"http://125.131.193.18:3000/clients/dispatch_ymon";
        readonly int            _timerInterval      = 3000;
        readonly int            _requestTimeout     = 3000;

		public Service()
		{
			InitializeComponent();

			// Initialize timer.
			_timer.Enabled  = true;
			_timer.Interval = _timerInterval;
			_timer.Elapsed  += new System.Timers.ElapsedEventHandler(timer_Tick);
		}

		protected override void OnStart(string[] args)
		{
			//_tw = new System.IO.StreamWriter("c:\\ymon.txt");
			_timer.Start();
		}

		protected override void OnStop()
		{
			_timer.Stop();
			//_tw.Close();
		}

		protected override void OnPause()
		{
			base.OnPause();
			_timer.Start();
		}

		protected override void OnContinue()
		{
			base.OnContinue();
			_timer.Start();
		}

        private string _SendRequest()
        {
            // prepare request
            WebRequest  req = WebRequest.Create(_requestUri);            

            string  postData    = _data.ToString();
            byte[]  byteArray   = Encoding.UTF8.GetBytes(postData);

            req.Method          = "post";
            req.ContentType     = "text/xml";
            req.ContentLength   = byteArray.Length;
            req.Timeout         = _requestTimeout;

            System.IO.Stream    dataStream;
            try
            {
                //System.IO.Stream    dataStream  = req.GetRequestStream();
                dataStream  = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            HttpWebResponse res = null;
            try
            {
                res = (HttpWebResponse)req.GetResponse();    // send request
            }
            catch (WebException e)
            {
                res.Close();
                return e.Message;
            }

            // get result
            dataStream      = res.GetResponseStream();
            System.IO.StreamReader  sr  = new System.IO.StreamReader(dataStream);
            string      resString   = sr.ReadToEnd();
            sr.Close();
            dataStream.Close();

            res.Close();

            return resString;
        }

		private void timer_Tick(object sender, EventArgs e)
		{
            ++_tickCounter;

            _data.AppendStatus(Convert.ToInt32(SystemMonitor.CpuUsage), Convert.ToInt32(SystemMonitor.AvailableMemory), DateTime.Now);

            if (_tickCounter >= _requestInterval)
            {
                _tickCounter    = 0;
                //if (_tw != null)
                //{
                //    _tw.WriteLine(_data.ToString());
                //    _tw.WriteLine(String.Format("RESULT : {0}", _SendRequest()));
                //    _tw.Flush();
                //}

                _SendRequest();

                _data.ClearStatus();
            }
		}
	}
}
