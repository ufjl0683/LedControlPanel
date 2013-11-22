using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LedClientService.Devices;
using CeraDevices.Packages;

namespace LedClientService.Devices
{
  
    [DataContract]
    public class LedDevice:Device
    {
        System.Collections.Generic.Dictionary<OutputPriority, LedOutputData> outqueue = new Dictionary<OutputPriority, LedOutputData>();
        
        // CIRCLE RECTANGLE
        // [DataMember]
         //public  string Shape { get; set; }
        //LED TRIGer  COLOR MONO
         [DataMember]
         public string LedType { get; set; }
         [DataMember]
         public int W { get; set; }
         [DataMember]
         public int R { get; set; }
         [DataMember]
         public int G { get; set; }
         [DataMember]
         public int B { get; set; }
        
        public  void SetOutput(LedOutputData outdata)
         {
             if (!outqueue.ContainsKey(outdata.Priority))
                 outqueue.Add(outdata.Priority, outdata);
             else
             {
                 outqueue.Remove(outdata.Priority);
                 outqueue.Add(outdata.Priority, outdata);
             }

             Output();
         }

        public LedOutputData GetCurrentOutput()
        {
            if (outqueue.Count == 0)
            {
                return new LedOutputData() { SectionID=this.SectionID, B = 100, R = 100, W = 100, G = 100, Priority = OutputPriority.Default };
            }
            else
            {
                List<LedOutputData> list = outqueue.Values.ToList<LedOutputData>();
                list.Sort();
                // call communication , notify slPanel to Change Color
                return list[0];
            }
        }

        public void RemoveOutput(LedOutputData data)
        {
            if (this.outqueue.ContainsKey(data.Priority))
                outqueue.Remove(data.Priority);
            Output();
        }
        public void Output()
        {
            try
            {

                List<LedOutputData> list = outqueue.Values.ToList<LedOutputData>();
                list.Sort();
                LedOutputData outdata=this.GetCurrentOutput();
                LedControlPackage pkg;
           
                     pkg = new LedControlPackage((ushort)this.ZeeBeeID, (byte)1, (byte)outdata.W, (byte)outdata.R, (byte)outdata.G, (byte)outdata.B);
                    
               
                pkg.OnFailed += (s, a) =>
                    {
                         this.IsConnected = false;
                         lock (Service1.MainControl.outDev)
                         {
                             System.Threading.Monitor.Pulse(Service1.MainControl.outDev);
                         }
                    };

                pkg.OnCompleted += (s, a) =>
                    {
                        this.IsConnected = true;
                        lock (Service1.MainControl.outDev)
                        {
                            System.Threading.Monitor.Pulse(Service1.MainControl.outDev);
                        }
                    };
                 lock (Service1.MainControl.outDev)
                {
                Service1.MainControl.outDev.SendPackage(pkg);
                System.Threading.Monitor.Wait(Service1.MainControl.outDev, 1000 * 60);
                }

                bool bDisplayChanged = this.W != outdata.W || this.R != outdata.R || this.G != outdata.G || this.B!=outdata.B;
                 this.W = outdata.W;
                 this.R = outdata.R;
                 this.G = outdata.G;
                 this.B = outdata.B;
               // if(bDisplayChanged)
                     Service1.ControlService.NotifyClientLedDisplayChange(this.ZeeBeeID,outdata );
               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "," + ex.StackTrace);
            }
            // call communication , notify slPanel to Change Color

           
        }


    }
}
