using CeraDevices;
using CeraDevices.Packages;
using LED;
using System;
using System.Collections.Generic;
using wpfPanel;

namespace CommTest
{
    class Program
    {
        static object lockobj = new object();
        static System.Collections.Generic.List<ushort> addr = new List<ushort>();
        static void Main(string[] args)
        {


            Config config = new Config();
            config.Coordinators = new CoordinatorConfig[] { new CoordinatorConfig() { ID = 0, BaseUrl="http://10.10.1.1:8080" } };
            config.Groups = new GroupConfig[]{
                new GroupConfig{ GroupID=0, GroupName="TestGroup", DimLevel=0 ,Devices=new DeviceConfig[]{new DeviceConfig(){ CoordinatorID=0, MAC="ABCDEFG", RmkID="aabb" } }   }
            };

            Config.WriteXml(config,"config.xml");
            
           CeraDevices.CoordinatorDevice cdev = new CeraDevices.CoordinatorDevice("10.10.1.1", 8080);
           
        //   CeraDevices.CorrdinatorInfo info= cdev.GetDeviceInfo();
         // StreetLightInfo[] list= cdev.GetStreetLightList();

        //   CeraDevices.DeviceManager mgr = new DeviceManager(new CoordinatorDevice[] { cdev });
            
            
           //Console.WriteLine(info.ToString());
           //CeraDevices.DeviceInfo[] infos = cdev.GetDeviceList();
           //cdev.SetStreetLightRemark("8814","AABBCC");

          //  mgr["aabb"].SetDeviceDimLevel(mgr.GetDeviceID("aabb"), 80);
         //   mgr["aabb"].SetDeviceDimLevel(mgr.GetDeviceID("aabb"), 0);
       
           // CeraDevices.CoordinatorDevice.PermitJoinNode(60);
          
           cdev.SetDeviceSchedule("*", "0,180,1190,1210,1235,0,0,0,0,0", "20,40,60,20,20,255,255,255,255,255");
            cdev.SetDeviceRTC("*", DateTime.Now);
     //       cdev.SetDeviceEnableSch("8814", true);

            cdev.SetDeviceScheduleEnable("*", true);

            CeraDevices.StreetLightInfo[] stifos = cdev.GetStreetLightList();

Console.ReadKey();
cdev.SetDeviceScheduleEnable("*", false);
foreach (StreetLightInfo info in stifos)
{
    cdev.SetDeviceDimLevel(info.DevID, 100);
}
Console.ReadKey();

        }


        static void CeraMicroDeviceTest(string[] args)
         {

             CmdBasePackage pkg2, pkg1;

             if (args.Length == 0)
             {
                 Console.WriteLine("請指定 ComPort,請按任意鍵繼續");
                 Console.ReadKey();
                 return;
             }

             CeraDevices.CeraDevice dev = new CeraDevices.CeraDevice(args[0], 115200);
             int successcnt = 0;
             dev.OnChildTableReport += new ChildTableReportHandler(dev_OnChildTableReport);

             pkg2 = new PermitNoteJoinPackage(30);
             pkg2.OnCompleted += (s, a) =>
             {
                 Console.WriteLine("開放加入Node 30 sec 命令成功,請按任意鍵繼續");

             }
             ;
             pkg2.OnFailed += (s, a) =>
             {
                 Console.WriteLine("開放加入Node 30 sec 命令失敗,請按任意鍵繼續");
                 Console.ReadKey();
                 return;
             };
             dev.SendPackage(pkg2);
             //  Console.WriteLine("開放加入Node 30 sec,請按任意鍵繼續");


             Console.ReadKey();




             pkg1 = new CeraDevices.Packages.RequestChildrenTable();

             pkg1.OnCompleted += (s, a) =>
             {
                 Console.WriteLine("要求所有的裝置回報成功,請按任意鍵繼續 ");

             };
             pkg1.OnFailed += (s, a) =>
             {
                 Console.WriteLine("要求所有的裝置回報失敗,請按任意鍵繼續");
                 Console.ReadKey();
                 return;
             };
             dev.SendPackage(pkg1);
             Console.ReadKey();
             //pkg1.OnCompleted += (s, a) =>
             //    {
             //        //CmdBasePackage pkg2 = new LedControlPackage(0x1000, 1,100, 0, 0, 0);
             //        //dev.SendPackage(pkg2);
             //    };

             //====================================================================
             Console.WriteLine("準備開始測試,請按任意鍵繼續");
             Console.ReadKey();
             bool flag = true;

             while (true)
             {
                 for (int i = flag ? 0 : 100; flag ? i <= 100 : i >= 0; )
                 {
                     //  int[] add = new int[] { 0x100b, 0x1016, 0x1008, 0x1003 };

                     for (int j = 0; j < addr.Count; j++)
                     {
                         LedControlPackage pkg = new LedControlPackage(addr[j], 01, (byte)i, (byte)i, (byte)i, (byte)i);

                         pkg.OnFailed += (s, a) =>
                         {
                             Console.WriteLine("failed");
                             lock (lockobj)
                                 System.Threading.Monitor.Pulse(lockobj);
                         };

                         pkg.OnCompleted += (s, a) =>
                         {
                             Console.WriteLine("Success");
                             lock (lockobj)
                                 System.Threading.Monitor.Pulse(lockobj);
                         };
                         dev.SendPackage(pkg);
                         lock (lockobj)
                             System.Threading.Monitor.Wait(lockobj, 1000);
                         // System.Threading.Thread.Sleep(100);
                     }

                     //System.Console.WriteLine("================" + i + "================");
                     //System.Threading.Thread.Sleep(3000);

                     if (flag) i += 10; else i -= 10;

                 }

                 flag = !flag;




             }
         }
        static void dev_OnChildTableReport(ResponseChildTable childTable)
        {

            Console.WriteLine("{0:X4}", childTable.Dest);
            addr.Add(childTable.Dest);
          //  Console.WriteLine(childTable.ToString());
            //throw new NotImplementedException();
        }

        class CallBack : ServiceReference1.IControlServiceCallback
        {
            public void SayHello(string hello)
            {
              //  throw new NotImplementedException();
            }

            public void ToClientSayHello(string hello)
            {
              //  throw new NotImplementedException();
            }

            public void ToClientNotifyLedDisplayChange(int DeviceID, ServiceReference1.LedOutputData outdata)
            {
              //  throw new NotImplementedException();
            }
        }
        static void crctest()
        {
              Crc16Ccitt crc16 = new Crc16Ccitt(InitialCrcValue.NonZero1);

          ushort res=  crc16.ComputeChecksum(new byte[]
            {
                0x18,0x11,0xEF,0xCD,0xAB,
                0x90,0x78,0x56,0x34,
                0x12,0x01,0x00,0x00,
                0x01,0x33,0x00,0x00,0x00,
                0x10,0xE3,0x0C,0xD7

            }
       );
            Console.WriteLine("{0:X4}",res);
            res = crc16.ComputeChecksum(new byte[]
            {
                0x18,0x11,0xEF,0xCD,0xAB,
                0x90,0x78,0x56,0x34,
                0x12,0x01,0x00,0x00,
                0x01,0x33,0x00,0x00,0x00,
                0x10,0xE3,0x0C,0xD7

            }
       );
            Console.WriteLine("{0:X4}", res);
            Console.ReadKey();
        }
    }
}
