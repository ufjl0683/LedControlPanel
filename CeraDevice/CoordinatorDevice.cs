using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 

namespace CeraDevices
{
     
  public    class CoordinatorDevice
    {
      private   string UriBase="http://10.10.1.1:8080";
   //     MyWebClient wc = new MyWebClient();
       

      public CoordinatorDevice(string ip, int port)
      {
          UriBase = "http://" + ip + ":" + port;
      }

      public CoordinatorDevice(string baseUrl)
      {
          UriBase = baseUrl;
      }
      public   CorrdinatorInfo GetDeviceInfo()
      {
          MyWebClient wc = new MyWebClient();
           System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(CorrdinatorInfo));

           Stream stream;
           CorrdinatorInfo retInfo =jsonsr.ReadObject(stream=wc.OpenRead(UriBase + "/info")) as CorrdinatorInfo;
           stream.Close();
           stream.Dispose();
           return retInfo;
       }

      public void SetDeviceEnableSch(string devid,bool enable)
      {
          MyWebClient wc = new MyWebClient();

          using (Stream stream = wc.OpenRead(UriBase + "/sstreet_light.set_dev_schedule?dev=" + devid + "&enable=" + (enable ? 1 : 0)))
          {
              while (stream.ReadByte() != -1) ;
          }
        //  10.10.1.1:8080/street_light.set_dev_schedule?dev=081f&enable=0 
      }

      public void SetDeviceRTC(string devid, DateTime dt)
      {
         //10.10.1.1:8080/street_light.set_rtc? dev=8201&rtc=13-08-31-12-10-11 
          MyWebClient wc = new MyWebClient();
          Stream stream;
          if(devid=="*")
            stream=  wc.OpenRead(UriBase + "/street_light.set_rtc?rtc=" + dt.ToString("yy-MM-dd-HH-mm-ss"));
          else
           stream=  wc.OpenRead(UriBase + "/street_light.set_rtc?dev=" +devid+"&rtc="+dt.ToString("yy-MM-dd-HH-mm-ss"));
               
               while(stream.ReadByte()!=-1);
               stream.Close();
               stream.Dispose();

      }
      public   void PermitJoinNode(int secs)
      {
          MyWebClient wc = new MyWebClient();

         using (Stream stream=wc.OpenRead(UriBase + "/permit_join_node?seconds=" + secs.ToString()))
         {
             while(stream.ReadByte()!=-1);
         }
    
      }

      public   void ChangeRFChanel(int chanelno)
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
      public   void Kick(string dev_id)
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

      public   void AddDeviceByMAC(string MAC)
      {
//          Command /add_device /add_device /add_device
//Parameters mac string MAC address
//Response Body - - -
//Add device 0x4d38100301020304: Add device 0x4d38100301020304: Add device 0x4d38100301020304:
//10.10.1.1:8080/add_dev_by_mac?mac=4d38100301020304 
          MyWebClient client=new MyWebClient();
          using (Stream stream = client.OpenRead(UriBase + "/add_dev_by_mac?mac=" + MAC))
          {
              while (stream.ReadByte() != -1) ;
          }
          
         
      }

      public    DeviceInfo[] GetDeviceList()
      {

          MyWebClient wc = new MyWebClient();
          
          System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(DeviceInfoList));
//          Command /get_dev_list /get_dev_list /get_dev_list
//Parameters - - -
//Response Body {  list: [  {    addr: “”,    type: “”,    pan: “”,    mac: “”,    visibility: 1,  },...] }
//array(object)
//string string string string int
//device id
//device id device type device pan id MAC Address visibility
          DeviceInfoList infolist = null;
          using (Stream stream = wc.OpenRead(UriBase + "/get_dev_list"))
          {
              infolist = jsonsr.ReadObject(stream) as DeviceInfoList; ;
          }
          return infolist.list;

      }

      public async Task< DeviceInfo[]> GetDeviceListAsync()
      {


          System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(DeviceInfoList));
          //          Command /get_dev_list /get_dev_list /get_dev_list
          //Parameters - - -
          //Response Body {  list: [  {    addr: “”,    type: “”,    pan: “”,    mac: “”,    visibility: 1,  },...] }
          //array(object)
          //string string string string int
          //device id
          //device id device type device pan id MAC Address visibility
          DeviceInfoList infolist = null;
          MyWebClient wc = new MyWebClient();
          using (Stream stream =await wc.OpenReadTaskAsync(UriBase + "/get_dev_list"))
          {
              infolist = jsonsr.ReadObject(stream) as DeviceInfoList; ;
          }
          return infolist.list;

      }

      public    StreetLightInfo[]  GetStreetLightList()

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
           
         System.Runtime.Serialization.Json.DataContractJsonSerializer jsonsr = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(StreetLightInfoList));

         StreetLightInfoList infolist = null;
       
         using (Stream stream =      wc.OpenRead (UriBase + "/street_light.get_dev_list"))
         {
             infolist = jsonsr.ReadObject(stream) as StreetLightInfoList; ;
         }
         int len = infolist.list.Length;
         return infolist.list.Take(len-1).ToArray();

    }


      public async Task<StreetLightInfo[]> GetStreetLightListAsync()
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
      public void SetDeviceDimLevel(string devid, int level)
      {
          MyWebClient client = new MyWebClient();
          using (Stream stream =   client.OpenRead(new Uri(UriBase + "/street_light.set_dim_level?dev=" + devid + "&level=" + level, UriKind.Absolute)))
          {
              //  while (stream.ReadByte() != -1) ;
              System.Diagnostics.Debug.Print(UriBase + "/street_light.set_dim_level?dev=" + devid + "&level=" + level);

          }
      }
      public  async  void    SetDeviceDimLevelAsync(string devid, int level)
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

          //lock (this)
          //{
             MyWebClient client = new MyWebClient();
              using (Stream stream =  await client.OpenReadTaskAsync(new Uri(UriBase + "/street_light.set_dim_level?dev=" + devid + "&level=" + level, UriKind.Absolute)))
              {
                         //  while (stream.ReadByte() != -1) ;
                  System.Diagnostics.Debug.Print(UriBase + "/street_light.set_dim_level?dev=" + devid + "&level=" + level);

              }
             
           //  client.Dispose();
          //}     
         // client.Dispose();
         
      }
      public void SetStreetLightRemark(string devid,string remark)
      {
          MyWebClient wc = new MyWebClient();
          string result="";
          if (remark.Length > 64)
              result = remark.Substring(0, 64);
          else
              result = remark.PadRight(64, '0');
          using (Stream stream=wc.OpenRead(UriBase + "/street_light.set_remark?dev="+devid+"&remark="+result))
          {
              while (stream.ReadByte() != -1) ;
          }
          

      }

     
     
    }

     


        [DataContract]
        public class StreetLightInfoList{

            [DataMember]
          public  StreetLightInfo[] list;
        }

        [DataContract]
        public class StreetLightInfo
        {
            [DataMember]
            [Display(AutoGenerateField=false)]
             public string dev {get;set;}
             [DataMember]
             [Display(AutoGenerateField = false)]
            public string lt {get;set;}  //last report time
            [DataMember]
            [Display(AutoGenerateField = false)]
            public bool to{get;set;}  //time out
            [DataMember]
            [Display(AutoGenerateField = false)]
            public int rr{get;set;}  //rsi receive
            [DataMember]
            [Display(AutoGenerateField = false)]
            public int  rt{get;set;}   //rsi transmit
            [DataMember]
          
            public  string rtc{get;set;}
            [DataMember]
            [Display(AutoGenerateField = false)]
            public int?[] l { get; set; } //dim level
            [DataMember]
            [Display(AutoGenerateField = false)]
            public int?[] r { get; set; } //sensor
            [DataMember]
            
            public int?[] t { get; set; } //temperature
            [DataMember]
            public Schedule sch { get; set; }
            [DataMember]
            [Display(AutoGenerateField = false)]
            public PowerMeter[] pm { get; set; }
            [DataMember]
            [Display(AutoGenerateField = false)]
            public string cmt { get; set; }

           [Display(Order=1)]
            public string DevID{
                get
                {
                    return dev;
                }
            }
             [Display(Order = 2)]
            public string  RmkID
            {
                get
                {
                    return DevID;  //MAC.Substring(12, 4);
                  //  return cmt.Substring(0, 4);
                }
            }
             public string MAC
             {
                 get
                 {
                     return cmt.Substring(4, 16);
                 }

             }

             [Display(Order = 3)]
            public int CurrentDimLevel
            {
                get
                {
                    return (int)l[0];
                }
            }
             [Display(Order = 4)]
            public  double V 
            {
                get
                {
                   return   pm[0].v * 1e-6;
                }
             }
             [Display(Order = 5)]
            public double A
            {
                get
                {
                   return   pm[0].a * 1e-6;
                }
            }
             [Display(Order = 6)]
            public double W
            {
                get
                {
                    return pm[0].w * 1e-6;
                }
            }
 [Display(Order = 7)]
            public double PF
            {
                get
                {
                    return pm[0].pf * 1e-6;
                }
            }
             [Display(Order = 8)]
            public double KWHP
            {
                 get
                {
                   return   pm[0].kwh_p* 1e-3;
                }

            }
             [Display(Order = 9)]
            public double KWHN
            {
                get
                {
                    return pm[0].kwh_n * 1e-3;
                }

            }
             [Display(Order = 10)]
            public double F
            {
                get
                {
                    return pm[0].f * 1e-2;
                }

            }
            
            
        }

        [DataContract]
        public class PowerMeter
        {
            // voltage  current  walt  power factor  frequency  apparent power  kwh positive  kwh negative
            //v: 0,      a: 0,      w: 0,      pf: 0,      f: 0,      ap: 0,      kwh_p: 0,      kwh_n: 0  
             [DataMember]
            public int v{get;set;}  //unit 1e-6
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
            public int kwh_p { get; set; }//1e-3 scale
             [DataMember]
            public int kwh_n { get; set; }// 1e-3 scale
            

        }
        [DataContract]
        public class Schedule
        {
           //  enable: 0,      time: [0,...],      level: [0,...]  
            [DataMember]
            public bool enable { get; set; }
              [DataMember]
            public int?[] time { get; set; }
              [DataMember]
            public int?[] level { get; set; }
        }

  [DataContract]
  public class DeviceInfoList
  {
      [DataMember]
     public  DeviceInfo[] list;

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
      public bool visibility { get; set; }

  }
    [DataContract]
    public class  CorrdinatorInfo
    {
     // {  build: “”,  servertime: “”,  pan_id: “”,  mac_address: “”,  rf_channel }
     [DataMember]
        public string build { get; set; }
          [DataMember]
        public string pan_id { get; set; }
          [DataMember]
        public string mac_address { get; set; }
          [DataMember]
        public string rf_channel { get; set; }

    }
    
}
