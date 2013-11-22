using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LedClientService.Devices
{
    public  enum   OutputPriority
    {


        Default=-2,
        Scheduled = -1,
        OneTime = 0
    }
    
     [DataContract]
     public  class LedOutputData:IComparable<LedClientService.Devices.LedOutputData>
    {
          [DataMember]
         public int W {get;set;} 
          [DataMember]
         public int R {get;set;}
          [DataMember]
         public int G { get; set; }
          [DataMember]
         public int B { get; set; }
          [DataMember]
         public OutputPriority Priority{get;set;}
         [DataMember]
         public int SectionID { get; set; }
         public int CompareTo(LedOutputData other)  //desending
         {
             return other.Priority - this.Priority;
            // throw new NotImplementedException();
         }
    }
}
