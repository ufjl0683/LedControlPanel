using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {


            DateTime dt = System.Convert.ToDateTime("2015-10-10T20:00:00");
            Console.WriteLine(dt.ToLongDateString());
            Console.ReadKey();

            //OfficeDevices.IProjector projector = new OfficeDevices.HitachiProjector("192.168.0.168", 23);

            //Console.WriteLine("------------------list------------------");
            //foreach (string s in projector.GetSignalSourceName())
            //    Console.WriteLine(s);

            //Console.WriteLine("------------------poweron------------------");
            //projector.PowerON();
            //Console.ReadLine();
            //Console.WriteLine("------------------status------------------");
            //Console.WriteLine(projector.GetPowerStatus());
            //   Console.ReadLine();

            //   foreach (string s in projector.GetSignalSourceName())
            //   {
            //       Console.WriteLine("------------------Set video source------------------");
            //         projector.SetSignalSource(s);
            //         Console.ReadLine();
            //   }

            //   Console.WriteLine("------------------Poweroff------------------");
            //projector.PowerOFF();
            //Console.ReadLine();


            //Console.WriteLine(projector.GetPowerStatus());
            //Console.ReadLine();

          
        }

        static void CommandTest()
        {
            //SendCommand("BEEF03060019D3020000600000");//get power
            //Console.ReadLine();

            SendCommand("BEEF030600BAD2010000600100"); //poweron
            Console.ReadLine();

            //SendCommand("BEEF030600CDD2020000200000");//get current  source
            //Console.WriteLine();


            SendCommand("BEEF030600FED2010000200000");
            Console.ReadLine();
            SendCommand("BEEF0306002AD3010000600000");// power off
            Console.ReadLine();
        }
        public static byte[] StringToByteArray(String hex)
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

        static string SendCommand(String value)
        {

            Int32 port = 23;
            String hostname = "192.168.0.168";
            String responseData = String.Empty;
            try
            {

                TcpClient client = new TcpClient(hostname, port);



                Byte[] data = StringToByteArray(value);

                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                data = new Byte[8];
                System.Threading.Thread.Sleep(300);
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

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
