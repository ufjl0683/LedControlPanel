using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LedClientService.Devices
{
     public delegate void OnConnectedChangeHandler(Device dev);
    [DataContract]
    public class Device
    {
        public event OnConnectedChangeHandler OnConnectChanged;

        private bool _IsConnected = true;
    
        [DataMember]
        public string DeviceType { get; set; }
        [DataMember]
        public int DeviceID { get; set; }
        //zeebee mac
         [DataMember]
        public int ZeeBeeID { get; set; }

         [DataMember]
         public int SectionID { get; set; }

         [DataMember]
         public bool IsConnected {
             get
             {
                 return _IsConnected;
             }
             set
             {
                 if (value != _IsConnected)
                 {
                     _IsConnected = value;
                     if (this.OnConnectChanged != null)
                         this.OnConnectChanged(this);
                 }
             }
         }

    }
}
