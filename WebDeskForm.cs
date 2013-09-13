using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WebDesk
{
    public partial class WebDeskForm : Form
    {
        HttpServer.Server server;
        
        public WebDeskForm()
        {
            InitializeComponent();

            server = new HttpServer.Server();

            var fm = new HttpServer.Modules.FileModule();
            fm.Resources.Add(new HttpServer.Resources.FileResources("/", ".\\"));
            //fm.AddDefaultMimeTypes();
            server.Add(fm);
            
            var cgi = new CgiModule();
            server.Add(cgi);

            var listener = HttpServer.HttpListener.Create(System.Net.IPAddress.Loopback, 0);
            server.Add(listener);

            server.Start(64);
            
            MessageBox.Show(listener.Port.ToString());
            webBrowser.Url = new Uri("http://127.0.0.1:" + listener.Port + "/index.html");
        }

        private void WebDeskForm_Load(object sender, EventArgs e)
        {
            //webBrowser.Document.Write("<html><body><b>Test!</b></body></html>");
            //webBrowser.Document.Write("<b>Test! <a href='http://google.com/'>aaa</a></b>");
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            /*MessageBox.Show(e.Url.ToString());
            e.Cancel = true;
            webBrowser.Document.
            webBrowser.Document.Write("<i>Test22222!</i>");*/
        }


    }
}
