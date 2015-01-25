using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CeraDevices
{
    public  class MyWebClient:WebClient
    {
        int Timeout = 5000;

        //public MyWebClient():base()
        //{
            

        //}

        protected override WebRequest GetWebRequest(Uri address)
        {
            var objWebRequest = base.GetWebRequest(address);
            objWebRequest.Timeout = this.Timeout;
            return objWebRequest;
        }



    }
}
