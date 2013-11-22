using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CeraDevices.Packages
{
  public  class CoordinatorAck:CmdBasePackage
    {
     public  CoordinatorAck(byte[] data)
      {
          this.PackageData = data;
      }

     public bool IsSucess
     {
         get
         {
             if (PackageData[1] == 0xff)
                 return true;
             else
                 return false;
         }
     }
    }
}
