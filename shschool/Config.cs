using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace shschool
{

    [DataContract]
   public  class Config
    {
        [DataMember]
      public  StreetLightBindingData[] StreetLightBindingDatas { get; set; }
        [DataMember]
        public System.Collections.Generic.List<Scenarior> Scenariors { get; set; }

    }



    public class Scenarior
    {
        public string SceneName { get; set; }
        public System.Collections.Generic.List<StreetLightBindingData> List { get; set; }
    }
}
