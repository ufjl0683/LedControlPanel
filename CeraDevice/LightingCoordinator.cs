using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
 

namespace CeraDevices.LightingCoordinator
{

    public class Coordinator: ILightingCoordinator
    {
        private string UriBase = "http://10.10.1.1:8080";
        //     MyWebClient wc = new MyWebClient();


        public Coordinator(string ip, int port)
        {
            UriBase = "http://" + ip + ":" + port;
        }

        public Coordinator(string baseUrl)
        {
            UriBase = baseUrl;
        }
        public  virtual  CorrdinatorInfo GetDeviceInfo()   //version 2
        {
            MyWebClient wc = new MyWebClient();
            System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(CorrdinatorInfoBase));

            Stream stream;
            CorrdinatorInfoBase retInfo = jsonsr.ReadObject(stream = wc.OpenRead(UriBase + "/info")) as CorrdinatorInfoBase;
            stream.Close();
            stream.Dispose();
            return retInfo.info;
        }

        //public void SetDeviceEnableSch(string devid,bool enable)
        //{
        //    MyWebClient wc = new MyWebClient();

        //    using (Stream stream = wc.OpenRead(UriBase + "/sstreet_light.set_dev_schedule?dev=" + devid + "&enable=" + (enable ? 1 : 0)))
        //    {
        //        while (stream.ReadByte() != -1) ;
        //    }
        //  //  10.10.1.1:8080/street_light.set_dev_schedule?dev=081f&enable=0 
        //}


        public virtual StreetLightInfo[] GetVisibleStreetLightList()
        {
            DeviceInfo[] devlist = GetDeviceList();
 
            devlist = devlist.Where(n => n != null && n.visibility==1).ToArray();
 
            StreetLightInfo[] streetlist = GetStreetLightList().ToArray();

            StreetLightInfo[] retList = (from m in streetlist join n in devlist on m.DevID equals n.addr select m).ToArray();

            return retList;
        }
        public virtual async Task<StreetLightInfo[]> GetVisibleStreetLightListAsync()
        {
            DeviceInfo[] devlist = await GetDeviceListAsync();

 
            devlist = devlist.Where(n => n != null && n.visibility==1).ToArray();
 
            StreetLightInfo[] streetlist = await GetStreetLightListAsync();
            foreach (DeviceInfo dev in devlist)
            {
                StreetLightInfo info = streetlist.Where(n => n.DevID == dev.addr).FirstOrDefault();
                if (info != null)
                    info.MAC = dev.mac;
            }
            StreetLightInfo[] retList = (from m in streetlist join n in devlist on m.DevID equals n.addr select m ).ToArray();

            return retList;
        }
        public virtual void SetDeviceRTC(string devid, DateTime dt)
        {
            //10.10.1.1:8080/street_light.set_rtc? dev=8201&rtc=13-08-31-12-10-11 
            MyWebClient wc = new MyWebClient();
            Stream stream;
            if (devid == "*")
                stream = wc.OpenRead(UriBase + "/lighting.set_rtc?rtc=" + dt.ToString("yy-MM-dd-HH-mm-ss"));
            else
                stream = wc.OpenRead(UriBase + "/lighting.set_rtc?dev=" + devid + "&rtc=" + dt.ToString("yy-MM-dd-HH-mm-ss"));

            while (stream.ReadByte() != -1) ;
            stream.Close();
            stream.Dispose();



        }

        public virtual async Task SetDeviceRTCAsync(string devid, DateTime dt)
        {
            //10.10.1.1:8080/street_light.set_rtc? dev=8201&rtc=13-08-31-12-10-11 
            MyWebClient wc = new MyWebClient();
            Stream stream;
            if (devid == "*")
                stream = await wc.OpenReadTaskAsync(UriBase + "/lighting.set_rtc?rtc=" + dt.ToString("yy-MM-dd-HH-mm-ss"));
            else
                stream = await wc.OpenReadTaskAsync(UriBase + "/lighting.set_rtc?dev=" + devid + "&rtc=" + dt.ToString("yy-MM-dd-HH-mm-ss"));

            while (stream.ReadByte() != -1) ;
            stream.Close();
            stream.Dispose();
           


        }


        public virtual void SetDeviceSchedule(string devid, string timeStr, string levelStr)
        {

            MyWebClient wc = new MyWebClient();
            DateTime dt = DateTime.Now;
            Stream stream;
            if (devid == "*")
            {
                stream = wc.OpenRead(UriBase + "/lighting.set_dev_schedule?time=" + timeStr + "&level=" + levelStr/*+"&on="+dt.Year+"-"+dt.Month+"-"+dt.Day*/);
            }
            else
            {
                stream = wc.OpenRead(UriBase + "/lighting.set_dev_schedule?dev=" + devid + "&time=" + timeStr + "&level=" + levelStr /* + "&on=" + dt.Year + "-" + dt.Month + "-" + dt.Day*/);
            }
            while (stream.ReadByte() != -1) ;
            stream.Close();
            stream.Dispose();

        }

        public virtual async void SetDeviceScheduleAsync(string devid, string timeStr, string levelStr)
        {

            MyWebClient wc = new MyWebClient();
            
            Stream stream;
            System.DateTime dt = DateTime.Now;
            if (devid == "*")
            {
                stream = await wc.OpenReadTaskAsync(new Uri(UriBase + "/lighting.set_dev_schedule?time=" + timeStr + "&level=" + levelStr/*+"&on="+dt.Year+"-"+dt.Month+"-"+dt.Day*/));

               
            }
            else
            {
                stream = await wc.OpenReadTaskAsync(UriBase + "/lighting.set_dev_schedule?dev=" + devid + "&time=" + timeStr + "&level=" + levelStr/* + "&on=" + dt.Year + "-" + dt.Month + "-" + dt.Day*/);
            }
            while (stream.ReadByte() != -1) ;
            stream.Close();
            stream.Dispose();

        }

        public virtual async void SetDeviceScheduleEnableAsync(string devid, bool enable)
        {
            //10.10.1.1:8080/street_light.set_dev_schedule?dev=081f&enable=0 

            MyWebClient wc = new MyWebClient();
            Stream stream;
            if (devid == "*")
            {
                stream = await wc.OpenReadTaskAsync(UriBase + "/lighting.set_dev_schedule?enable=" + (enable ? "1" : "0"));
            }
            else
            {
                stream = await wc.OpenReadTaskAsync(UriBase + "/lighting.set_dev_schedule?dev=" + devid + "&enable=" + (enable ? "1" : "0"));
            }
            while (stream.ReadByte() != -1) ;
            stream.Close();
            stream.Dispose();
        }

        public virtual void SetDeviceScheduleEnable(string devid, bool enable)
        {
            //10.10.1.1:8080/street_light.set_dev_schedule?dev=081f&enable=0 

            MyWebClient wc = new MyWebClient();
            Stream stream;
            if (devid == "*")
            {
                stream = wc.OpenRead(UriBase + "/lighting.set_dev_schedule?enable=" + (enable ? "1" : "0"));
            }
            else
            {
                stream = wc.OpenRead(UriBase + "/lighting.set_dev_schedule?dev=" + devid + "&enable=" + (enable ? "1" : "0"));
            }
            while (stream.ReadByte() != -1) ;
            stream.Close();
            stream.Dispose();
        }
        public void PermitJoinNode(int secs)
        {
            MyWebClient wc = new MyWebClient();

            using (Stream stream = wc.OpenRead(UriBase + "/permit_join_node?seconds=" + secs.ToString()))
            {
                while (stream.ReadByte() != -1) ;
            }

            //System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Coordinator2.CoordinatorResult));

            //Stream stream;
            //Coordinator2.CoordinatorResult res = jsonsr.ReadObject(stream = wc.OpenRead(UriBase + "/info")) as CorrdinatorInfoBase;

        }

        public virtual void ChangeRFChanel(int chanelno)
        {
            //           /change_rf_channel /change_rf_channel /change_rf_channel
            //Parameters channel int channel index
            //Response Body - - -
            //Example Change RF channel to 13: Change RF channel to 13: Change RF channel to 13:Example
            //10.10.1.1:8080/change_rf_channel?channel=13 
            //    
            MyWebClient wc = new MyWebClient();
            using (Stream stream = wc.OpenRead(UriBase + "/change_rf_channel?channel=" + chanelno.ToString()))
            {
                while (stream.ReadByte() != -1) ;
            }

        }
        public virtual void Kick(string dev_id)
        {
            //          Command /kick /kick /kick
            //Parameters name string device id
            //Response Body - - -
            //Remove device 8011 Remove device 8011 Remove device 8011
            //10.10.1.1:8080/kick?name=8011 
            MyWebClient wc = new MyWebClient();

            using (Stream stream = wc.OpenRead(UriBase + "/kick?name=" + dev_id))
            {
                while (stream.ReadByte() != -1) ;
            }
        }

        public virtual void AddDeviceByMAC(string MAC)
        {
            //          Command /add_device /add_device /add_device
            //Parameters mac string MAC address
            //Response Body - - -
            //Add device 0x4d38100301020304: Add device 0x4d38100301020304: Add device 0x4d38100301020304:
            //10.10.1.1:8080/add_dev_by_mac?mac=4d38100301020304 
            MyWebClient client = new MyWebClient();
            using (Stream stream = client.OpenRead(UriBase + "/add_dev_by_mac?mac=" + MAC))
            {
                while (stream.ReadByte() != -1) ;
            }


        }

        public virtual DeviceInfo[] GetDeviceList()  //version 2
        {

            MyWebClient wc = new MyWebClient();

            System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(DeviceInfoListBase));
            //          Command /get_dev_list /get_dev_list /get_dev_list
            //Parameters - - -
            //Response Body {  list: [  {    addr: “”,    type: “”,    pan: “”,    mac: “”,    visibility: 1,  },...] }
            //array(object)
            //string string string string int
            //device id
            //device id device type device pan id MAC Address visibility
            DeviceInfoListBase infolist = null;
            using (Stream stream = wc.OpenRead(UriBase + "/get_dev_list"))
            {
                infolist = jsonsr.ReadObject(stream) as DeviceInfoListBase; ;
            }
            return  infolist.list.Where(n => n != null).ToArray();

        }



        public virtual async Task<DeviceInfo[]> GetDeviceListAsync()  //version3
        {


            System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(DeviceInfoListBase));
            //          Command /get_dev_list /get_dev_list /get_dev_list
            //Parameters - - -
            //Response Body {  list: [  {    addr: “”,    type: “”,    pan: “”,    mac: “”,    visibility: 1,  },...] }
            //array(object)
            //string string string string int
            //device id
            //device id device type device pan id MAC Address visibility
            DeviceInfoListBase infolist = null;
            MyWebClient wc = new MyWebClient();
            using (Stream stream = await wc.OpenReadTaskAsync(UriBase + "/get_dev_list"))
            {
                infolist = jsonsr.ReadObject(stream) as DeviceInfoListBase; ;
            }
            return infolist.list.Where(n => n != null).ToArray();

        }

        public virtual StreetLightInfo[] GetStreetLightList()   //version 2
        {
            return GetStreetLightList("*");
        }
        public virtual StreetLightInfo[] GetStreetLightList(string devid)  //version 2
        {

            //         Command /street_light.get_dev_list /street_light.get_dev_list /street_light.get_dev_list
            //Parameters dev (optional) string device id
            //Response Body {  list: [  {    dev: “”,    lt: “2013-03-01-15:00:00”,    to: true,    rr: 0,    rt: 0,    rtc: “13-03-01-15-33-32”,    l: [0,...],    r: [0,...],    t: [0,...],    sch: {      enable: 0,      time: [0,...],      level: [0,...]    },    pm: [    {      v: 0,      a: 0,      w: 0,      pf: 0,      f: 0,      ap: 0,      kwh_p: 0,      kwh_n: 0    },...],    cmt: “000000....”  },...] }
            //array(object)
            //string string boolean int int string array(int) array(int) array(int) object int int int
            //array(object)
            //int int int int int int int int
            //hex[64]
            //device status
            //device id last report time time out RSSI Rx (%) RSSI Tx (%) rtc time dim level light sensor temp. sensor dimming schedule  boolean  time code  dim level
            //power meter
            //  voltage  current  walt  power factor  frequency  apparent power  kwh positive  kwh negative
            //comment
            MyWebClient wc = new MyWebClient();

            System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(StreetLightInfoListBase));

            StreetLightInfoListBase infolist = null;
            if (devid == "*")
                using (Stream stream = wc.OpenRead(UriBase + "/lighting.get_dev_list"))
                {
                    Console.WriteLine(UriBase + "/lighting.get_dev_list");
                    infolist = jsonsr.ReadObject(stream) as StreetLightInfoListBase; ;
                }
            else
            {
                using (Stream stream = wc.OpenRead(UriBase + "/lighting.get_dev_list?dev=" + devid))
                {
                    infolist = jsonsr.ReadObject(stream) as StreetLightInfoListBase; ;
                }
            }
            int len = infolist.list.Length;

           //StreetLightInfo [] schedinfo=  GetDeviceSchedule(devid);
          //  foreach(StreetLightInfo info in infolist
           StreetLightInfo[] retinfos = infolist.list.Take(len - 1).ToArray();
           //foreach (StreetLightInfo info in retinfos)
           //{
           //    StreetLightInfo temp = schedinfo.Where(n => n.dev == info.dev).FirstOrDefault();
           //    if (temp == null)
           //        continue;
           //    info.sch = temp.sch;
           //    info.cmt = temp.cmt;
           //    info.SetScheduleEnable(temp.IsScheduleEnable);

           //}
           return retinfos; //infolist.list.Take(len - 1).ToArray();

        }


        private  async Task<StreetLightInfo[]> GetDeviceScheduleAsync(string dev)
        {


            return await Task.FromResult<StreetLightInfo[]>(GetDeviceSchedule(dev));

            //System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(StreetLightInfoList));

            //StreetLightInfoList infolist = null;
            //using (Stream stream =  await wc.OpenReadTaskAsync(UriBase + "/street_light.get_dev_list"))
            //{
            //    infolist = jsonsr.ReadObject(stream) as StreetLightInfoList; ;
            //}
            //int len = infolist.list.Length;
            //return infolist.list.Take(len - 1).ToArray();


        }

        private  StreetLightInfo[] GetDeviceSchedule(string devid)  //version 2
        {
            MyWebClient wc = new MyWebClient();

            System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(StreetLightInfoListBase));

            StreetLightInfoListBase infolist = null;
            if (devid == "*")
                using (Stream stream = wc.OpenRead(UriBase + "/lighting.get_dev_info"))
                {
                    Console.WriteLine(UriBase + "/lighting.get_dev_info");
                    infolist = jsonsr.ReadObject(stream) as StreetLightInfoListBase; ;
                }
            else
            {
                using (Stream stream = wc.OpenRead(UriBase + "/lighting.get_dev_info?dev=" + devid))
                {
                    infolist = jsonsr.ReadObject(stream) as StreetLightInfoListBase; ;
                }
            }
            int len = infolist.list.Length;
            return infolist.list.Take(len - 1).ToArray();
        }

        //[DataContract]
        //public class DeviceScheduleBase
        //{
        //    [DataMember(Name="objects")]
        //    StreetLightInfo[] list { get; set; }
        //}

        public virtual async Task<StreetLightInfo[]> GetStreetLightListAsync()  //version2
        {


            return await Task.FromResult<StreetLightInfo[]>(GetStreetLightList());

            //System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(StreetLightInfoList));

            //StreetLightInfoList infolist = null;
            //using (Stream stream =  await wc.OpenReadTaskAsync(UriBase + "/street_light.get_dev_list"))
            //{
            //    infolist = jsonsr.ReadObject(stream) as StreetLightInfoList; ;
            //}
            //int len = infolist.list.Length;
            //return infolist.list.Take(len - 1).ToArray();


        }

        public virtual async Task<StreetLightInfo[]> GetStreetLightListAsync(string devid)
        {


            return await Task.FromResult<StreetLightInfo[]>(GetStreetLightList(devid));

            //System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(StreetLightInfoList));

            //StreetLightInfoList infolist = null;
            //using (Stream stream =  await wc.OpenReadTaskAsync(UriBase + "/street_light.get_dev_list"))
            //{
            //    infolist = jsonsr.ReadObject(stream) as StreetLightInfoList; ;
            //}
            //int len = infolist.list.Length;
            //return infolist.list.Take(len - 1).ToArray();


        }
        public virtual void SetDeviceDimLevel(string devid, int level)
        {
            MyWebClient client = new MyWebClient();
            string args;
            //if(devid=="*")
            //    args = "/street_light.set_dim_level?level=" + level;
            //else
            args = "/lighting.set_dim_level?dev=" + devid + "&level=" + level;
            using (Stream stream = client.OpenRead(new Uri(UriBase + args, UriKind.Absolute)))
            {
                while (stream.ReadByte() != -1) ;
                //    System.Diagnostics.Debug.Print(UriBase + "/street_light.set_dim_level?dev=" + devid + "&level=" + level);

            }
        }
        public virtual async void SetDeviceDimLevelAsync(string devid, int level)
        {
            //          /street_light.set_dim_level /street_light.set_dim_level /street_light.set_dim_level
            //Parameters dev (optional) string device id Parameters
            //level int dim level
            //Response Body - - -
            //Remark dim level = 0 (off), 20~100 (level in %) dim level = 0 (off), 20~100 (level in %) dim level = 0 (off), 20~100 (level in %)
            //Example Set dimming level of device 8020 to 85% Set dimming level of device 8020 to 85% Set dimming level of device 8020 to 85%Example
            //10.10.1.1:8080/street_light.set_dim_level?dev=8020&level=85 
            //if(devid=="*")
            //    using (Stream stream = wc.OpenRead(UriBase + "/street_light.set_dim_level?level=" + level))
            //    {
            //        while (stream.ReadByte() != -1) ;
            //    }
            //else
            string args;
            //lock (this)
            //{
            MyWebClient client = new MyWebClient();
            //if (devid == "*")
            //    args = "/street_light.set_dim_level?level=" + level;
            //else
            args = "/lighting.set_dim_level?dev=" + devid + "&level=" + level;
            using (Stream stream = await client.OpenReadTaskAsync(new Uri(UriBase + args, UriKind.Absolute)))
            {
                while (stream.ReadByte() != -1) ;
                // System.Diagnostics.Debug.Print(UriBase + "/street_light.set_dim_level?dev=" + devid + "&level=" + level);

            }

            //  client.Dispose();
            //}     
            // client.Dispose();

        }
        public virtual void SetStreetLightRemark(string devid, string remark)
        {
            MyWebClient wc = new MyWebClient();
            string result = "";
            if (remark.Length > 64)
                result = remark.Substring(0, 64);
            else
                result = remark.PadRight(64, '0');
            using (Stream stream = wc.OpenRead(UriBase + "/lighting.set_remark?dev=" + devid + "&remark=" + result))
            {
                while (stream.ReadByte() != -1) ;
            }


        }
        public  static string ToASCIIString(string str)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(str);
            string result = "";

            foreach (byte b in data)
            {
                result += b.ToString("X2");
            }
            return result;
        }




        //CeraDevices.CorrdinatorInfo ICoordinatorDevice.GetDeviceInfo()
        //{
        //    throw new NotImplementedException();
        //}

        //CeraDevices.DeviceInfo[] ICoordinatorDevice.GetDeviceList()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<CeraDevices.DeviceInfo[]> ICoordinatorDevice.GetDeviceListAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //CeraDevices.StreetLightInfo[] ICoordinatorDevice.GetStreetLightList()
        //{
        //   // throw new NotImplementedException();
        //    return GetStreetLightList("*");
        //}

        //CeraDevices.StreetLightInfo[] ICoordinatorDevice.GetStreetLightList(string devid)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<CeraDevices.StreetLightInfo[]> ICoordinatorDevice.GetStreetLightListAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<CeraDevices.StreetLightInfo[]> ICoordinatorDevice.GetStreetLightListAsync(string devid)
        //{
        //    throw new NotImplementedException();
        //}

        //CeraDevices.StreetLightInfo[] ICoordinatorDevice.GetVisibleStreetLightList()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<CeraDevices.StreetLightInfo[]> ICoordinatorDevice.GetVisibleStreetLightListAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }




    [DataContract]
    public class StreetLightInfoList
    {

        [DataMember]
        public StreetLightInfo[] list;
    }
    [DataContract]
    public class StreetLightInfoListBase
    {
        [DataMember]
        public bool success { get; set; }
        [DataMember(Name="objects")]
        public StreetLightInfo[] list { get; set; }
    }

    [DataContract]
    public class StreetLightInfo
    {
      

        [Display(Order = 1)]
        public string DevID
        {
            get
            {
                return dev;
            }
        }
        //[Display(Order = 2)]
        //public string RmkID
        //{
        //    get
        //    {
        //        return DevID;  //MAC.Substring(12, 4);
        //        //  return cmt.Substring(0, 4);
        //    }
        //}
        [DataMember(Name = "mac")]
        public string MAC
        {


            get;
            set;
            //get
            //{
            //    return cmt.Substring(4, 16);
            //}

        }

        //[Display(Order = 3)]
        //public int CurrentDimLevel
        //{
        //    get
        //    {
        //        return w;
        //    }
        //}
        //[Display(Order = 4)]
        //public double V
        //{
        //    get
        //    {
        //        return pm[0].v * 1e-6;
        //    }
        //    set
        //    {
        //        pm[0].v = (int)(value / 1e-6);
        //    }
        //}
        //[Display(Order = 5)]
        //public double A
        //{
        //    get
        //    {
        //        return pm[0].a * 1e-6;
        //    }
        //    set
        //    {
        //        pm[0].a = (int)(value / 1e-6);
        //    }
        //}
            [DataMember(Name = "pan")]
        public string PAN
        {
            get;
            set;
        }

         [DataMember(Name = "w")]
         public int CurrentDimLevel //dimlevel
        {
            get;
            set;
        }
        //[Display(Order = 7)]
        //public double PF
        //{
        //    get
        //    {
        //        return pm[0].pf * 1e-6;
        //    }

        //    set
        //    {

        //        pm[0].pf = (int)(value / 1e-6);
        //    }
        //}
        //[Display(Order = 8)]
        //public double KWHP
        //{
        //    get
        //    {
        //        return pm[0].kwh_p * 1e-3;
        //    }
        //    set
        //    {
        //        pm[0].kwh_p = (uint)(value / 1e-3);
        //    }

        //}
        //[Display(Order = 9)]
        //public double KWHN
        //{
        //    get
        //    {
        //        return pm[0].kwh_n * 1e-3;
        //    }

        //    set
        //    {
        //        pm[0].kwh_n = (uint)(value / 1e-3);
        //    }

        //}
        //[Display(Order = 10)]
        //public double F
        //{
        //    get
        //    {
        //        return pm[0].f * 1e-2;
        //    }

        //    set
        //    {
        //        pm[0].f = (int)(value / 1e-2);
        //    }

        //}
         
        [DataMember(Name = "visibility")]
         public int Visible
         {
             get;
             set;
         }

        
        //public double Temperature
        //{
        //    get
        //    {
        //        return (t[0] ?? -999) / 100.0;
        //    }
        //}

        //public double LightSensor  //0~100
        //{
        //    get
        //    {
        //        return (r[0] ?? -999) / 100.0;
        //    }
        //}

        //public bool IsScheduleEnable
        //{
        //    get
        //    {
        //        return this.sch.en;
        //    }
        //}

       

        [DataMember]
        [Display(AutoGenerateField = false)]
        public string dev { get; set; }
        [DataMember]
        [Display(AutoGenerateField = false)]
        public string lt { get; set; }  //last report time
        [DataMember]
        [Display(AutoGenerateField = false)]
        public bool to { get; set; }  //time out
        [DataMember]
        [Display(AutoGenerateField = false)]
        public int rr { get; set; }  //rsi receive
        [DataMember]
        [Display(AutoGenerateField = false)]
        public int rt { get; set; }   //rsi transmit
        [DataMember]

        public string rtc { get; set; }
        //[DataMember]
        //[Display(AutoGenerateField = false)]
        //public int?[] l { get; set; } //dim level
        //[DataMember]
        //[Display(AutoGenerateField = false)]
        //public int?[] r { get; set; } //sensor
        //[DataMember]

        //public int?[] t { get; set; } //temperature
        //[DataMember]
        //public Schedule sch { get; set; }
        //[DataMember]
        //[Display(AutoGenerateField = false)]
        //public PowerMeter[] pm { get; set; }
        //[DataMember]
        //[Display(AutoGenerateField = false)]
        //public string cmt { get; set; }

        //public void SetScheduleEnable(bool enable)
        //{
        //    this.sch.en = enable;
        //}
        //public string GetScheduleSegTimeString()
        //{

        //    return this.sch.GetScheduleSegTimeString();
        //    //string timestr;
        //    //timestr = sch.Segnments[0].Time.ToString();
        //    //for (int i = 1; i < sch.Segnments.Length; i++)
        //    //    timestr += "," + sch.Segnments[i].Time;


        //    //return timestr.TrimEnd(new char[] { ',' }) ;
        //}

        //public string GetScheduleSegLevelString()
        //{
        //    return this.sch.GetScheduleSegLevelString();
        //    //string levelstr;
        //    //levelstr = sch.Segnments[0].Level.ToString();
        //    //for (int i = 1; i < sch.Segnments.Length; i++)
        //    //    levelstr += "," + sch.Segnments[i].Level;
        //    //return levelstr.TrimEnd(new char[]{','});

        //}


    }

    [DataContract]
    public class PowerMeter
    {
        // voltage  current  walt  power factor  frequency  apparent power  kwh positive  kwh negative
        //v: 0,      a: 0,      w: 0,      pf: 0,      f: 0,      ap: 0,      kwh_p: 0,      kwh_n: 0  
        [DataMember]
        public int v { get; set; }  //unit 1e-6
        [DataMember]
        public int a { get; set; }  //unit 1e-6
        [DataMember]
        public int w { get; set; }  //unit 1e-6
        [DataMember]
        public int pf { get; set; }  //unit 1e-6
        [DataMember]
        public int f { get; set; }  //1e-2
        [DataMember]
        public int ap { get; set; }
        [DataMember]
        public uint kwh_p { get; set; }//1e-3 scale
        [DataMember]
        public uint kwh_n { get; set; }// 1e-3 scale


    }
    [DataContract]
    public class Schedule
    {


        //  enable: 0,      time: [0,...],      level: [0,...]  
        [DataMember]
        public bool en { get; set; }
        [DataMember]
        public int?[] time { get; set; }
        [DataMember]
        public int?[] level { get; set; }


        private ScheduleSegnment[] segs;

        public bool IsEqual(Schedule otherSch)
        {
            ScheduleSegnment[] thisseg = Segnments.Where(n => n.Level != 255).OrderBy(n => n.Time).ToArray();
            ScheduleSegnment[] otherseg = otherSch.Segnments.Where(n => n.Level != 255).OrderBy(n => n.Time).ToArray();

            if (thisseg.Length != otherseg.Length)
                return false;
            bool iseq = true;
            try
            {
                for (int i = 0; i < thisseg.Length; i++)
                {
                    if (thisseg[i].Time == 15555)
                        continue;
                    iseq = iseq && thisseg[i].Time == otherseg[i].Time && thisseg[i].Level == otherseg[i].Level;
                }
            }
            catch (Exception ex)
            {
                iseq = false;
            }

            return iseq;

        }
        public ScheduleSegnment[] Segnments
        {
            get
            {
                if (segs != null)
                    return segs;
                if (time == null || level == null || time.Length == 0 || level.Length == 0)
                    return new ScheduleSegnment[0];
                else
                {

                    segs = new ScheduleSegnment[time.Length - 1];
                    for (int i = 0; i < time.Length - 1; i++)
                    {
                        if (time[i] != null)
                            segs[i] = new ScheduleSegnment() { Time = (int)time[i], Level = (int)level[i] };
                    }


                    return segs;
                }
            }

            set
            {
                segs = value;
            }
        }

        public string GetScheduleSegTimeString()
        {
            string timestr;
            timestr = this.Segnments[0].Time.ToString();
            for (int i = 1; i < this.Segnments.Length; i++)
                timestr += "," + this.Segnments[i].Time;


            return timestr.TrimEnd(new char[] { ',' });
        }
      


        //public string GetScheduleSegTimeString()
        //{
        //    string timestr;
        //    timestr = this.Segnments[0].Time.ToString();
        //    for (int i = 1; i < this.Segnments.Length; i++)
        //        timestr += "," + this.Segnments[i].Time;


        //    return timestr.TrimEnd(new char[] { ',' });
        //}

        public string GetScheduleSegLevelString()
        {
            string levelstr;
            levelstr = this.Segnments[0].Level.ToString();
            for (int i = 1; i < this.Segnments.Length; i++)
                levelstr += "," + this.Segnments[i].Level;
            return levelstr.TrimEnd(new char[] { ',' });

        }
    }

    public class ScheduleSegnment : INotifyPropertyChanged
    {
        int _time = 15555;
        int _level = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        public int Time
        {
            get
            {
                return _time;
            }
            set
            {
                if (value != _time)
                {
                    //System.Text.RegularExpressions.Regex regx = new System.Text.RegularExpressions.Regex(@"\d{1,2}:\d{1,2}");
                    //if (!regx.Match(value.ToString()).Success)
                    //    throw new ArgumentException("format err");
                    //int hour = value / 60;
                    //int min = value % 60;
                    if (value == -1)
                        throw new ArgumentException("format err");
                    _time = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Time"));


                }

            }
        }
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value != _level)
                {
                    int res;

                    //if(!int.TryParse(value.ToString(),out res))
                    //    throw new Exception("format err");
                    //if(!(value>=0 && value<=100 || value==255))
                    //    throw new Exception("format err");

                    _level = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Level"));


                }

            }

        }
    }

    [DataContract]
    public class DeviceInfoListBase
    {
       [DataMember]
        public bool success { get; set; }

      
        [DataMember(Name = "objects")]
       public DeviceInfo[] list;

    }

    [DataContract]
    public class DeviceInfoList
    {
        [DataMember]
        public DeviceInfo[] list;

    }


    [DataContract]
    public class DeviceInfo
    {
        [DataMember]
        public string addr { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string pan { get; set; }
        [DataMember]
        public string mac { get; set; }
        [DataMember]
 
        public int visibility { get; set; }

 

    }
    [DataContract]
    public class CorrdinatorInfoBase
    {
        // {  build: “”,  servertime: “”,  pan_id: “”,  mac_address: “”,  rf_channel }
        [DataMember]
        public bool success { get; set; }
        [DataMember(Name="object")]
        public CorrdinatorInfo info { get; set; }

    }

      [DataContract]
    public class CorrdinatorInfo
    {

        [DataMember]
        public string build { get; set; }
        [DataMember]
        public string pan_id { get; set; }
        [DataMember]
        public string mac_address { get; set; }
        [DataMember]
        public string rf_channel { get; set; }
    }

      [DataContract]
      public class CoordinatorResult
      {
          [DataMember]
          public bool success { get; set; }
      }

    public      interface ILightingCoordinator
    {
        void AddDeviceByMAC(string MAC);
        void ChangeRFChanel(int chanelno);
          CorrdinatorInfo GetDeviceInfo();
        DeviceInfo[] GetDeviceList();
        System.Threading.Tasks.Task<DeviceInfo[]> GetDeviceListAsync();
         StreetLightInfo[] GetStreetLightList();
        StreetLightInfo[] GetStreetLightList(string devid);
          System.Threading.Tasks.Task<StreetLightInfo[]> GetStreetLightListAsync();
        System.Threading.Tasks.Task<StreetLightInfo[]> GetStreetLightListAsync(string devid);
         StreetLightInfo[] GetVisibleStreetLightList();
        System.Threading.Tasks.Task<StreetLightInfo[]> GetVisibleStreetLightListAsync();
        void Kick(string dev_id);
        void PermitJoinNode(int secs);
        void SetDeviceDimLevel(string devid, int level);
        void SetDeviceDimLevelAsync(string devid, int level);
        void SetDeviceRTC(string devid, DateTime dt);
        System.Threading.Tasks.Task SetDeviceRTCAsync(string devid, DateTime dt);
        void SetDeviceSchedule(string devid, string timeStr, string levelStr);
        void SetDeviceScheduleAsync(string devid, string timeStr, string levelStr);
          void SetDeviceScheduleEnable(string devid, bool enable);
        void SetDeviceScheduleEnableAsync(string devid, bool enable);
        void SetStreetLightRemark(string devid, string remark);
    }
    }

