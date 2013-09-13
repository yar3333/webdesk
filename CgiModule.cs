using System;
using System.Collections.Generic;
using System.Text;

namespace WebDesk
{
    class CgiModule : HttpServer.Modules.IModule
    {
        public HttpServer.ProcessingResult Process(HttpServer.RequestContext context)
        {
            //context.Response.Body.Write();
            return HttpServer.ProcessingResult.SendResponse;
        }
    }
}
