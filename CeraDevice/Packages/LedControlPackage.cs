using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CeraDevices.Packages
{
    public   class LedControlPackage:CmdBasePackage
    {

        public byte W { get { return PackageData[7]; } }
        public byte R { get { return PackageData[8]; } }
        public byte G { get { return PackageData[9]; } }
        public byte B { get { return PackageData[10]; } }

        protected LedControlPackage()
        {
            this.ReturnCmd = 0xff;//  0xd0;// 0xff;
        }
        public LedControlPackage(byte[]  PackageData):this()
        {
            this.PackageData = PackageData;
            //this.DeviceType=0x52;

           
        }

        public LedControlPackage(ushort Src, byte devchannel, byte W, byte R, byte G, byte B):this()
        {
            byte[] cmddata = new byte[11];
           
            cmddata[0] = 0x50;
            cmddata[1 ] = (byte)(Src % 256);
            cmddata[2 ] = (byte)(Src / 256);
            cmddata[3 ] = devchannel;
            cmddata[4 ] = 0x52;  //type
            cmddata[5 ] = 0x81;   //sub type
            cmddata[6 ] = 0x2;    //sub cmd
            cmddata[7 ] = W;
            cmddata[8 ] = R;
            cmddata[9 ] = G;
            cmddata[10] = B;
           
             this.PackageData = cmddata;
           // this.ReturnCmd=0xd0;
        }

        public ushort SrcAddress
        {
            get
            {
                return (ushort)(PackageData[1] + PackageData[2] * 256);
            }
        }

        public byte DeviceChannel
        {
            get
            {
                return PackageData[3];
            }

        } // default 0
        public byte DeviceType  //0x52  for led
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
        public byte SubCmd
        {
            get
            {
                return PackageData[6];
            }
        }

    }
}
