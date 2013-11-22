using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CeraDevices.Packages
{
     public  class CmdBasePackage
    {
         public object Tag;
         public byte[] PackageData;
         public byte ReturnCmd = 0xff;
         public int SendCnt = 0;
         public CmdBasePackage ReturnPackage;
         public event EventHandler OnCompleted;
         public event EventHandler OnFailed;
         public byte Cmd
         {
             get { return PackageData[0]; }

         }
         public int Length
         {
             get
             {
                 if (PackageData == null)
                     return 0;
                 return PackageData.Length;
             }
         }

         public byte[] ToCmdBytes()
         {
             byte[] data = new byte[PackageData.Length + 2];
             data[0] = 0xcc;
             data[1] = (byte)PackageData.Length;
             System.Array.Copy(PackageData,0, data, 2, PackageData.Length);
             return data;
         }

         public override string ToString()
         {
             //return base.ToString();
             return string.Format("Cmd:{0:X2} ReturnCmd{1:X2} ", Cmd, ReturnCmd);
         }
         public void NotifyFail()
         {
             if (this.OnFailed != null)
                 this.OnFailed(this, null);
         }
         public void NotifyCompleted()
         {
             if (this.OnCompleted != null)
                 this.OnCompleted(this, null);
         }
    }
}
