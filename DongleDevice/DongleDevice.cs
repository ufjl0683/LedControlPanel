using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LED.DonglePackage;

namespace LED
{
    public class DongleDevice
    {
        const int SYNC = 0x16;
        System.IO.Ports.SerialPort port;
        Crc16Ccitt crc16 = new Crc16Ccitt( InitialCrcValue.NonZero1);
        Queue<SendPackage> SendBuffer = new Queue<SendPackage>();
        System.Threading.Thread RecriveThread;
       public DongleDevice(string ComPort, int baud)
        {

            port = new System.IO.Ports.SerialPort(ComPort, baud, System.IO.Ports.Parity.None);
            try
            {
                if (port.IsOpen)
                {
                    Console.WriteLine("port is opened");
                    Console.ReadKey();
                }
             //   port.DiscardInBuffer();
            //    port.DiscardOutBuffer();
               
                port.Open();
               // port.DtrEnable = true;
                port.RtsEnable = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
                Console.ReadKey();
               // Environment.Exit(-1);
            }

            this.RecriveThread = new System.Threading.Thread(ReceiveTask);
            RecriveThread.Start();
        }

       void ReceiveTask()
       {
           while (true)
           {
               int data = port.ReadByte();
               switch (data)
               {
                   case SYNC:
                       object ret = ReadPackage();
                       if (ret == null)
                           continue;
                       break;
               }
               Console.WriteLine("{0:X2}", data);
           }
       }


       object  ReadPackage()
       {
           int cmd;
           if (port.ReadByte() != 0x16)
               return null;
           int len = port.ReadByte();
           if (len == 4)   //Ack
           {
               return new AckPackage();
           }
           byte[] data = new byte[len + 1];
           
           int offset=1;
           int cnt=0;
           while(( cnt=  port.Read(data,offset,len))!=len)
           {
               len -= cnt;
           }
           ushort datacrc =(ushort) (port.ReadByte()*256+port.ReadByte());
            
           ushort crc=    crc16.ComputeChecksum(data);
           if (crc != datacrc)
               throw new CRCException();

           //will modi here
           return  new  ReceivePackage();
       }

        ~DongleDevice()
        {
            if (port.IsOpen)
                port.Close();
        }
    }

   public class CRCException : Exception
    {
       public CRCException(string message)
           : base(message)
       {
       }
       public CRCException()
           : base()
       {
       }
    }
}
