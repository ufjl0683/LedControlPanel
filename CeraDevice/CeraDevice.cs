using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using CeraDevices.Packages;

namespace CeraDevices
{
    public delegate void ChildTableReportHandler (ResponseChildTable childTable);
    public class CeraDevice
    {
        public string ComType;
        public string ComPort;
        public int baud;
        const int MAX_TRY_CNT = 3;
        const int TIMEOUT_MSEC =1000;
        public SerialPort com;
        System.Threading.Thread ReceiveThread;
        System.Threading.Thread SendThread;
        System.Collections.Generic.Queue<CmdBasePackage> sendQueue = new Queue<CmdBasePackage>();
        CmdBasePackage currentSendPkg;
        object SendQueueLock = new object();
        object WaitRespLock = new object();
        public event ChildTableReportHandler OnChildTableReport;
        
        public CeraDevice(string ComPort, int baud)
        {
         
            ComType = "COM";
            this.ComPort = ComPort;
            this.baud = baud;
            initialCom();
            ReceiveThread = new System.Threading.Thread(ReceiveTask);
            ReceiveThread.Start();
            SendThread = new System.Threading.Thread(SendTask);
            SendThread.Start();
            
        }


        void SendTask()
        {

            while (true)
            {

                if (sendQueue.Count == 0 && currentSendPkg == null)
                {
                    lock (this.SendQueueLock)
                    {
                        System.Threading.Monitor.Wait(this.SendQueueLock);

                    }
                }
                if (sendQueue.Count > 0)
                {
                    lock(this.sendQueue)
                    currentSendPkg = sendQueue.Dequeue();

                    while (currentSendPkg.SendCnt < MAX_TRY_CNT)
                    {
                        currentSendPkg.SendCnt++;
                        this.SendBytes(currentSendPkg.ToCmdBytes());
                        //if (currentSendPkg.ReturnCmd != 0xff)
                        //{
                            lock (WaitRespLock)
                            {
                                if (System.Threading.Monitor.Wait(this.WaitRespLock, TIMEOUT_MSEC))
                                                                
                                    break;
                             

                                // here means timeout

                            }

                        //}
                        //else  // not  a request
                                               
                        //    break;
                      

                       


                    }// while

                    if (currentSendPkg.SendCnt < MAX_TRY_CNT)
                    {
                        if (currentSendPkg.ReturnCmd != 0xff)
                            currentSendPkg.NotifyCompleted();
                        else
                        {
                            if ((currentSendPkg.ReturnPackage as CoordinatorAck).IsSucess)
                                currentSendPkg.NotifyCompleted();
                            else
                                currentSendPkg.NotifyFail();
                        }


                     
                    }
                    else
                    {
                        currentSendPkg.NotifyFail();
                      
                    }
                    currentSendPkg = null;

                } //if

            }
        }


        System.IO.Stream BaseStream
        {
            get
            {
                if (this.ComType == "COM")
                    return this.com.BaseStream;
                else
                    return null;
            }
        }

        void initialCom()
        {
            if (ComType == "COM")
            {
                if (com != null && com.IsOpen)
                {
                    com.Close();
                    com.Dispose();
                }

              
                com = new SerialPort(ComPort, baud, Parity.None, 8, StopBits.One);
              
                com.Open();
            }

          
           
        }

        void ReceiveTask()
        {
            while (true)
            {
                try
                {
                    int d=0;
                        try
                        {
                            d = BaseStream.ReadByte();
                        }  catch { initialCom(); }

                        if (d == 0xcc)
                        {


                            int length = 0;
                            try
                            {
                                length = BaseStream.ReadByte();
                            }
                            catch { initialCom(); }

                            byte[] package = new byte[length];
                            try
                            {
                                BaseStream.Read(package, 0, length);

                                Console.WriteLine(ToHexString(package));
                                CmdBasePackage pkg = GetPackage(package);
                                if (pkg == null)
                                    continue;
                                if (pkg.Cmd == 0x04 && this.OnChildTableReport != null)
                                             this.OnChildTableReport((ResponseChildTable)pkg);


                                if (pkg != null)
                                {
                                    Console.WriteLine(pkg);
                                    if(currentSendPkg!=null)
                                    if (pkg.Cmd == currentSendPkg.ReturnCmd)
                                    {
                                        currentSendPkg.ReturnPackage = pkg;
                                        lock (WaitRespLock)
                                            System.Threading.Monitor.Pulse(WaitRespLock);
                                    }
                                }
                                else
                                {// unknown command
                                    Console.WriteLine("Unknown"+ToHexString(package));
                                }
                            }
                            catch(Exception ex ){
                                Console.WriteLine(ex.Message+","+ex.StackTrace);
                                initialCom(); }




                        } // if
                        else
                        {
                            Console.WriteLine("Unknown byte");
                        }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
       



            }
        }


        public CmdBasePackage GetPackage(byte[] data)
        {
            switch (data[0])
            {
                case 0x04:
                    return   new  ResponseChildTable(data);
                case 0xd0:
                    return new LedControlAckPackage(data);
                case 0xff:
                    return new CoordinatorAck(data);
                    break;
                default:
                    Console.WriteLine("unknown ");
                    break;
            }

            return null;
        }
        public void SendPackage(CmdBasePackage pkg)
        {
            lock (this.sendQueue)
            {
                this.sendQueue.Enqueue(pkg);
            }
            lock (SendQueueLock)
            {
                System.Threading.Monitor.Pulse(SendQueueLock);
            }
           
           // com.fl
        }

        public void SendBytes(byte[] data)
        {
            try
            {
                this.BaseStream.Write(data, 0, data.Length);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
                initialCom();
            }
        }
        public static string ToHexString(byte[] d)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < d.Length; i++)
            
                sb.Append( string.Format("{0:X2} ",d[i]));

            return sb.ToString().Trim();
        }

    }
}
