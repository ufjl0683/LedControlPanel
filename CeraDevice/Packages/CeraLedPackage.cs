using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CeraDevices.Packages
{
  public abstract class CeraLedPackage:CmdBasePackage
    {
      //  byte _Cmd;
      //  ushort _SrcAddress;
      //  byte _DeviceChannel, _DeviceType, _SubType, _SubCmd;
      
        
    

      public   ushort SrcAddress
      {
          get {
              return (ushort)(PackageData[1] + PackageData[2] * 256);
          }
      }
      public   byte DeviceChannel{
            get
            {
                return PackageData[3];
            }

           } // default 0
         public   byte DeviceType  //0x52  for led
          {
              get
              {
                    return PackageData[4];
              }
          }
         public byte SubType
         {
             get
             {
                 return PackageData[5];
             }
         }
            // 0x81   for Led
        public  byte SubCmd
         {
             get
             {
                 return PackageData[6];
             }
         }

      

        //public byte ReturnCmd
        //{
        //    get
        //    {

        //    }
        //    set
        //    {
        //    }
        //}
       
    }
}
