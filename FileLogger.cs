using System;
using System.Collections.Generic;
using System.Text;

namespace WebDesk
{
    class FileLogger : HttpServer.Logging.ILogger
    {
        public void Debug(string message, Exception exception)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Debug(string message)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Error(string message, Exception exception)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Error(string message)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Fatal(string message, Exception exception)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Fatal(string message)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Info(string message, Exception exception)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Info(string message)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Trace(string message, Exception exception)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Trace(string message)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Warning(string message, Exception exception)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }

        public void Warning(string message)
        {
            var f = new System.IO.StreamWriter("WebDesk.log", true);
            f.WriteLine(message);
            f.Close();
        }
    }
}
