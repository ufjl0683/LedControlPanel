using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace StreetLightPanel
{
     [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
  public   class Service1:IService1
    {

       public async  Task<System.IO.Stream> GetImage(int x, int y, int z)
         {

             string baseUrl = "http://mt1.google.com/vt/lyrs=m@129&hl=zh-TW&x=";
             string url = baseUrl + x.ToString() + "&y=" + y.ToString() + "&z=" + z.ToString() + "&s=";
           //  string url = "http://10.11.3.101/mapviewer/mcserver?request=gettitle&format=PNG&zoomlevel={0}&mapcache=MAP103.MAP103&mx={1}&my={2}";
             //string urlstr = string.Format(url, z, x, y);
             // System.Net.WebHeaderCollection whc=new System.Net.WebHeaderCollection();

             //  whc.Add(urlstr);
             // MessageBox.Show(urlstr);
             System.Net.HttpWebRequest wreq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);// e(urlstr);
             //  System.IO.MemoryStream ms = new System.IO.MemoryStream();
             //wreq.Headers = whc;
             //   byte[] imagebytes=wreq.DownloadData(urlstr);
             //   ms.Write(imagebytes,0,imagebytes.Length);
             // wreq.Headers.Add(System.Net.HttpRequestHeader.Referer, "http://http://mt1.google.com");
             // wreq.Headers.Add("user-agent", @"Mozilla/5.0");
             wreq.UserAgent = "Mozilla/5.0";
           //  WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";
             WebResponse resp = await wreq.GetResponseAsync();
             return resp.GetResponseStream();
          //   return wreq.GetResponse().GetResponseStream();
             //  System.IO.Stream stream=  wc.OpenRead(string.Format(url, 0, 0, 0));


             //  return System.Drawing.Bitmap.FromStream(stream) as System.Drawing.Bitmap;

         }
    }
}
