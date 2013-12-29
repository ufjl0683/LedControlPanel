using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CeraDevices;

namespace StreetLightPanel
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
        public Schedule Schedule { get; set; }
    }
}
