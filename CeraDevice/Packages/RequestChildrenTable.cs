using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CeraDevices.Packages
{
   public class RequestChildrenTable:CmdBasePackage
    {
        public RequestChildrenTable()
        {
            PackageData = new byte[1];
           
            PackageData[0] = 0x23;
            ReturnCmd =  0xff;
        }
    }
}
