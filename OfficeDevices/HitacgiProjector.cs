using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDevices
{
   

    public class HitachiProjector : OfficeDevices.IProjector
    {

        string _IP;
        int _Port;

      public HitachiProjector(string ip, int port)
      {
          _IP = ip;
          _Port = port;
      }

      public void PowerON()
      {
          SendCommand("BEEF030600BAD2010000600100");
      }

      public string  GetPowerStatus()
      {
        byte[] result = SendCommand("BEEF03060019D3020000600000");//get power
     //  byte[] data = this.StringToByteArray(result);
        switch (result[1])
        {
            case 0:
            return "OFF";
            case 1:
            return "ON";
            case 2:
            return "COOL";
            default:
            return "UNKNOWN";
        }

      }
      public void PowerOFF()
      {
          SendCommand("BEEF0306002AD3010000600000");// power off
      }

        public  void SetSignalSource(string SourceName)
        {
            string[] source = GetSignalSourceName();
            switch (SourceName)
            {
                case "COMPUTER1":
                    SendCommand("BEEF030600FED2010000200000");
                    break;
                case "COMPUTER2":
                    SendCommand("BEEF0306003ED0010000200040");
                    break;
                case "VIDEO":
                    SendCommand("BEEF0306006ED3010000200100");
                    break;
                case "S_VIDEO":
                    SendCommand("BEEF0306009ED3010000200200");
                    break;
                case "COMPONENT":
                    SendCommand("BEEF030600AED1010000200500");
                    break;
                case "HDMI":
                    SendCommand("BEEF0306000ED2010000200300");
                    break;
                    

            }
        }

        public string[] GetSignalSourceName()
        {

            return  new string[]{ 
            "COMPUTER1","COMPUTER2","VIDEO","S_VIDEO","COMPONENT","HDMI"};
        }

      private   byte[] StringToByteArray(String hex)
      {
          int NumberChars = hex.Length / 2;
          byte[] bytes = new byte[NumberChars];
          using (var sr = new StringReader(hex))
          {
              for (int i = 0; i < NumberChars; i++)
                  bytes[i] =
                    Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
          }
          return bytes;
      }



       private byte[] SendCommand(String value)
      {

          Int32 port = _Port;
          String hostname = _IP;
          byte[] responseData=null;//= String.Empty;
          try
          {

              TcpClient client = new TcpClient(hostname, port);



              Byte[] data = StringToByteArray(value);

              NetworkStream stream = client.GetStream();
              stream.Write(data, 0, data.Length);

              data = new Byte[8];
              System.Threading.Thread.Sleep(300);
              Int32 bytes = stream.Read(data, 0, data.Length);
              responseData = data;

              stream.Close();
              client.Close();

          }
          catch (Exception ex)
          {
              Console.WriteLine(ex.ToString());
          }
          return responseData;
      }


    }
}
