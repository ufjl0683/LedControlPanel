using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace wpfPanel
{
    [Serializable]
   public  class Config
    {
        public CoordinatorConfig[] Coordinators
        {
            get;   set;
        }
        public GroupConfig[] Groups { get; set; }
        


       public static   void WriteXml(Config config,string PathFileName )
       {
           System.Xml.Serialization.XmlSerializer sr = new XmlSerializer(typeof(Config));
           using( System.IO.Stream stream=System.IO.File.Open(PathFileName, System.IO.FileMode.Create))
           {
           sr.Serialize(stream, config );
           }
       }

       public  static  Config ReadXml(string PathFileName)
       {
           System.Xml.Serialization.XmlSerializer sr = new XmlSerializer(typeof(Config));
          return sr.Deserialize(System.IO.File.OpenRead(PathFileName)) as Config;
          // throw new  NotImplementedException();
       }
    }

    [Serializable]
    public class GroupConfig:INotifyPropertyChanged
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }

        public DeviceConfig[] Devices { get; set; }
        int _DimLevel;
        public int DimLevel {
            get
            {
                return _DimLevel;
            }
            set
            {
                if (value != _DimLevel)
                {
                    _DimLevel = value;
                    if(PropertyChanged!=null)
                        PropertyChanged(this,new PropertyChangedEventArgs("DimLevel"));
                }
            }
        }

        //public string ImageUri { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [Serializable]
    public class CoordinatorConfig {
           
        public int ID { get; set; }

        public string BaseUrl { get; set; }
       
    }

    [Serializable]
    public class DeviceConfig
    {

        public int CoordinatorID { get; set; }
       
        public string RmkID { get; set; }
         
        public string MAC {get;set;}

    }
}
