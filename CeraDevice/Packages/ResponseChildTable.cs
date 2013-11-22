using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CeraDevices.Packages
{
   public   class ResponseChildTable:CmdBasePackage
    {

       public ResponseChildTable(byte[] package)
       {
           this.PackageData = package;
       }

       public ushort Src
       {
           get
           {
               return (ushort)(PackageData[1] + PackageData[2] * 256);
           }
       }

       public ushort Dest
       {
           get
           {
               return (ushort)(PackageData[11] + PackageData[12] * 256);
           }
       }

       public string MAC
       {
           get
           {
               StringBuilder sb = new StringBuilder();
               for (int i = 3+7; i >=3 ; i--)
               {
                   sb.Append(string.Format("{0:X2}",PackageData[i]));
               }
               return sb.ToString();
           }

       }


       public ushort PAN_ID
       {
           get
           {
               return (ushort)(PackageData[13] + PackageData[14] * 256);
           }
       }

       public byte RF_Channel
       {
           get
           {
               return PackageData[15];
           }
       }


       public override string ToString()
       {
           return base.ToString() +
               string.Format("Src:0x{0:X4}  MAC:{1} PanID:{2:X4} ,RF_Channel:{3} Dest:0x{4:X4}  \n", Src, MAC, PAN_ID, RF_Channel,Dest);

       }

    }
}
