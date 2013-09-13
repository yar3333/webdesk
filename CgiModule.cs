using System;
using System.Collections.Generic;
using System.Text;

namespace WebDesk
{
    class CgiModule : HttpServer.Modules.IModule
    {
        HttpServer.HttpListener listener;

        public CgiModule(HttpServer.HttpListener listener)
        {
            this.listener = listener;
        }
        
        public HttpServer.ProcessingResult Process(HttpServer.RequestContext context)
        {
            var log = new FileLogger();
            
            context.HttpContext.Logger.Trace("CgiModule.Process");
            
            var process = new System.Diagnostics.Process();
            //process.StartInfo.WorkingDirectory
            process.StartInfo.FileName = "index.cgi";
            //process.StartInfo.Arguments
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
 
             //DOCUMENT_ROOT="C:/Program Files (x86)/Apache Software Foundation/Apache2.2/htdocs"
             //GATEWAY_INTERFACE="CGI/1.1"
             //HOME="/home/SYSTEM"
             //HTTP_ACCEPT="text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
             //HTTP_ACCEPT_CHARSET="ISO-8859-1,utf-8;q=0.7,*;q=0.7"
             //HTTP_ACCEPT_ENCODING="gzip, deflate"
             //HTTP_ACCEPT_LANGUAGE="en-us,en;q=0.5"
             //HTTP_CONNECTION="keep-alive"
             //HTTP_HOST="example.com"
             //HTTP_USER_AGENT="Mozilla/5.0 (Windows NT 6.1; WOW64; rv:5.0) Gecko/20100101 Firefox/5.0"
             //PATH="/home/SYSTEM/bin:/bin:/cygdrive/c/progra~2/php:/cygdrive/c/windows/system32:..."
             //PATHEXT=".COM;.EXE;.BAT;.CMD;.VBS;.VBE;.JS;.JSE;.WSF;.WSH;.MSC"
             //PATH_INFO="/foo/bar"
             //PATH_TRANSLATED="C:\Program Files (x86)\Apache Software Foundation\Apache2.2\htdocs\foo\bar"
             //QUERY_STRING="var1=value1&var2=with%20percent%20encoding"
             //REMOTE_ADDR="127.0.0.1"
             //REMOTE_PORT="63555"
             //REQUEST_METHOD="GET"
             //REQUEST_URI="/cgi-bin/printenv.pl/foo/bar?var1=value1&var2=with%20percent%20encoding"
             //SCRIPT_FILENAME="C:/Program Files (x86)/Apache Software Foundation/Apache2.2/cgi-bin/printenv.pl"
             //SCRIPT_NAME="/cgi-bin/printenv.pl"
             //SERVER_ADDR="127.0.0.1"
             //SERVER_ADMIN="(server admin's email address)"
             //SERVER_NAME="127.0.0.1"
             //SERVER_PORT="80"
             //SERVER_PROTOCOL="HTTP/1.1"
             //SERVER_SIGNATURE=""
             //SERVER_SOFTWARE="Apache/2.2.19 (Win32) PHP/5.2.17"
             //SYSTEMROOT="C:\Windows"
             //TERM="cygwin"
             //WINDIR="C:\Windows"            
            
            
            
            process.StartInfo.EnvironmentVariables.Add("SERVER_NAME", listener.Address.ToString());
            process.StartInfo.EnvironmentVariables.Add("GATEWAY_INTERFACE", "CGI/1.1");
            
            process.StartInfo.EnvironmentVariables.Add("SERVER_PROTOCOL", context.Request.HttpVersion);
            process.StartInfo.EnvironmentVariables.Add("SERVER_PORT", listener.Port.ToString());
            process.StartInfo.EnvironmentVariables.Add("REQUEST_METHOD", context.Request.Method);
            process.StartInfo.EnvironmentVariables.Add("REQUEST_URI", context.Request.Uri.PathAndQuery);
            process.StartInfo.EnvironmentVariables.Add("PATH_INFO", context.Request.Uri.AbsolutePath); // path suffix, if appended to URL after program name and a slash.
            // PATH_TRANSLATED: corresponding full path as supposed by server, if PATH_INFO is present.
            process.StartInfo.EnvironmentVariables.Add("SCRIPT_NAME", context.Request.Uri.LocalPath); // relative path to the program, like /cgi-bin/script.cgi.
            process.StartInfo.EnvironmentVariables.Add("QUERY_STRING", context.Request.Uri.Query);
            process.StartInfo.EnvironmentVariables.Add("REMOTE_ADDR", context.HttpContext.RemoteEndPoint.Address.ToString()); // IP address of the client (dot-decimal).
            process.StartInfo.EnvironmentVariables.Add("REMOTE_PORT", context.HttpContext.RemoteEndPoint.Port.ToString());
            //process.StartInfo.EnvironmentVariables.Add("AUTH_TYPE", );  // identification type, if applicable.
            //process.StartInfo.EnvironmentVariables.Add("REMOTE_USER", ); // used for certain AUTH_TYPEs.
            //process.StartInfo.EnvironmentVariables.Add("REMOTE_IDENT", ); // see ident, only if server performed such lookup.
            if (context.Request.ContentType != null)
            {
                process.StartInfo.EnvironmentVariables.Add("CONTENT_TYPE", context.Request.ContentType.Value); // Internet media type of input data if PUT or POST method are used, as provided via HTTP header.
            }
            process.StartInfo.EnvironmentVariables.Add("CONTENT_LENGTH", context.Request.ContentLength.Value.ToString()); // similarly, size of input data (decimal, in octets) if provided via HTTP header.

            process.StartInfo.EnvironmentVariables.Add("DOCUMENT_ROOT", ".");
            
            // Variables passed by user agent (HTTP_ACCEPT, HTTP_ACCEPT_LANGUAGE, HTTP_USER_AGENT, HTTP_COOKIE and possibly others) contain values of corresponding HTTP headers and therefore have the same sense.
            foreach (var header in context.Request.Headers)
            {
                process.StartInfo.EnvironmentVariables.Add("HTTP_" + header.Name.Replace("-", "_").ToUpperInvariant(), header.HeaderValue);
            }

            foreach (System.Collections.DictionaryEntry env in process.StartInfo.EnvironmentVariables)
            {
                log.Debug("env " + env.Key + " = " + env.Value);
            }
            
            process.Start();

            var output = process.StandardOutput.ReadToEnd().Replace("\r", "");
            var endOfHeaders = output.IndexOf("\n\n");
            var outputHeaders = (endOfHeaders >= 0 ? output.Substring(0, endOfHeaders) : output).Split(new String[] { "\n" }, StringSplitOptions.None);
            var outputBody = endOfHeaders >=0 ? output.Substring(endOfHeaders + 2) : "";
            
           foreach (var header in outputHeaders)
           {
                var nameAndValue = header.Split(new char[] { ':' }, 2);
                context.Response.Add(new HttpServer.Headers.StringHeader(nameAndValue[0], nameAndValue[1]));
           }
            context.Response.ContentLength.Value = outputBody.Length;
            var outputBodyBytes = ASCIIEncoding.ASCII.GetBytes(outputBody);
            context.Response.Body.Write(outputBodyBytes, 0, outputBodyBytes.Length);

            process.WaitForExit();
            process.Close();
            
            //context.Response.Body.Write();
            return HttpServer.ProcessingResult.SendResponse;
        }
    }
}
