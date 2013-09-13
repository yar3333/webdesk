using System;
using System.Collections.Generic;
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

            var listener = HttpServer.HttpListener.Create(System.Net.IPAddress.Loopback, 0);
            listener.Logger = new FileLogger();
            server.Add(listener);
            
            var fm = new HttpServer.Modules.FileModule();
            fm.Resources.Add(new HttpServer.Resources.FileResources("/", ".\\"));
            server.Add(fm);
            
            var cgi = new CgiModule(listener);
            server.Add(cgi);

            server.Start(64);
            
            webBrowser.Url = new Uri("http://127.0.0.1:" + listener.Port + "/");
        }
    }
}
