using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CeraDevices.Packages
{
   public  class LedControlAckPackage:CmdBasePackage
    {

       public LedControlAckPackage(byte[] data)
       {
           this.PackageData = data;

       }


    }
}
