using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CeraDevices
{
    public   class DeviceManager
    {
        System.Collections.Generic.Dictionary<string, CoordinatorDevice> dictDevice=new Dictionary<string,CoordinatorDevice>();
        System.Collections.Generic.Dictionary<string, string> dictDeviceID=new Dictionary<string,string>();
        CoordinatorDevice[] Coordinators;
        public  DeviceManager(CoordinatorDevice[] CoordinatorDevices)
        {

            Coordinators = CoordinatorDevices;

            for (int i = 0; i < Coordinators.Length; i++)
            {
                StreetLightInfo[] list =     Coordinators[i].GetStreetLightList();
                foreach (StreetLightInfo info in list)
                {
                    if (info != null)
                    {
                        dictDevice.Add(info.RmkID, Coordinators[i]);
                        dictDeviceID.Add(info.RmkID, info.DevID);
                    }
                }

            }

        }


          

        public string GetDeviceID(string RmkID)
        {
            return dictDeviceID[RmkID];
        }

       public  CoordinatorDevice this[string rmkID]
        {
            get
            {
                return dictDevice[rmkID];
                
            }
             
        }

       


    }
}
