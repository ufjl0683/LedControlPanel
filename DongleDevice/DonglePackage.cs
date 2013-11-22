using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LED.DonglePackage
{
  public   class SendPackage
    {




    }
  public enum PackageType
  {
      SendPackage,
      ReceivePackage,
      AckPackage
  }
  public  class PackageBase 
    {
        public int Cmd;
        public byte[] Message;
        public byte[] MacAddress;
        public int CRC;
        public  PackageType PackageType ;
        

    }
  public class ReceivePackage:PackageBase
  {

      public ReceivePackage()
      {
          this.PackageType = DonglePackage.PackageType.ReceivePackage;
      }



  }

    public class AckPackage:ReceivePackage
    {
        public AckPackage()
        {
            this.PackageType = DonglePackage.PackageType.AckPackage;
        }
    }
  
}
