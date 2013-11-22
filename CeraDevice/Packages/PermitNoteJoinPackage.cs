using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CeraDevices.Packages
{
   public class PermitNoteJoinPackage : CmdBasePackage
    {
        public PermitNoteJoinPackage(ushort PermitSec)
        {
           this.ReturnCmd = 0xff;
           this.PackageData = new byte[3] { 0X0D, (byte)(PermitSec % 256), (byte)(PermitSec / 256) };
            
        }

    }
}
