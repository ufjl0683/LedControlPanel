using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LedClientService.Devices
{
    public class GroupSection
    {
        public System.Collections.Generic.List<Device> Devices = new List<Device>();
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        
    }
}
