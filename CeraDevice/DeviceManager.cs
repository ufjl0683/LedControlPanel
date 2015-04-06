using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CeraDevices.Coordinator2;
namespace CeraDevices
{
    public   class DeviceManager
    {
        System.Collections.Generic.Dictionary<string, ICoordinatorDevice> dictDevice=new Dictionary<string,ICoordinatorDevice>();
        System.Collections.Generic.Dictionary<string, string> dictDeviceID=new Dictionary<string,string>();
        ICoordinatorDevice[] Coordinators;
        public  DeviceManager(ICoordinatorDevice[] CoordinatorDevices)
        {


            Coordinators = CoordinatorDevices;

#if DEBUG
            return;
#endif
            for (int i = 0; i < Coordinators.Length; i++)
            {

                StreetLightInfo[] list =     Coordinators[i].GetStreetLightList();
                foreach (StreetLightInfo info in list)
                {
                    if (info != null)
                    {
                      //   if (!dictDevice.ContainsKey(info.DevID))
                            dictDevice.Add(info.DevID, Coordinators[i]);
                        // else
                            
                            
                       
                          
                        
                      //  dictDeviceID.Add(info.RmkID, info.DevID);
                    }
                }

            }

        }

        public async Task<StreetLightInfo[]> GetStreetLightListAsync()
        {
            List<StreetLightInfo> list = new List<StreetLightInfo>();
            foreach (CoordinatorDevice coor in Coordinators)
            {
                try
                {
                    list.AddRange(await coor.GetStreetLightListAsync());


                }
                catch (Exception ex)
                {
                    ;
                }
            }

            return list.ToArray();

        }
        public StreetLightInfo[]  GetStreetLightList()
        {
            List<StreetLightInfo> list = new List<StreetLightInfo>();
           
             foreach (ICoordinatorDevice coor in Coordinators)
             {
                 try
                 {
                    list.AddRange(coor.GetStreetLightList());


                 }
                 catch (Exception ex)
                 {
                     ;
                 }
             }

             return list.ToArray();

        }
        public  async Task< StreetLightInfo[]> GetStreetLightListAsync(string devid)
        {
            //List<StreetLightInfo> list = new List<StreetLightInfo>();

            //foreach (CoordinatorDevice coor in Coordinators)
            //{
            //    try
            //    {
            //        list.AddRange(coor.GetStreetLightList());


            //    }
            //    catch (Exception ex)
            //    {
            //        ;
            //    }
            //}


            return  await this[devid].GetStreetLightListAsync(devid); ;

        }
        public StreetLightInfo[] GetStreetLightList(string devid)
        {
            //List<StreetLightInfo> list = new List<StreetLightInfo>();

            //foreach (CoordinatorDevice coor in Coordinators)
            //{
            //    try
            //    {
            //        list.AddRange(coor.GetStreetLightList());


            //    }
            //    catch (Exception ex)
            //    {
            //        ;
            //    }
            //}


            return this[devid].GetStreetLightList(devid); ;

        }

        public string GetDeviceID(string RmkID)
        {
            return dictDeviceID[RmkID];
        }

       public  ICoordinatorDevice this[string devid]
        {
            get
            {
                if (dictDevice.ContainsKey(devid))
                    return dictDevice[devid];
                else
                    throw new Exception("devid not found!");
                
            }
             
        }





       public void SetDeviceRTC(string devid, DateTime dateTime)
       {
           if (devid == "*")
           {
               foreach (ICoordinatorDevice coor in Coordinators)
               {
                   try
                   {
                       coor.SetDeviceRTC("*", dateTime);
                   }
                   catch
                   {
                       ;
                   }
               }
           }
           else
           {
               try
               {
                   this[devid].SetDeviceRTC(devid, dateTime);
               }
               catch
               {
                   ;
               }
           }

         //  throw new NotImplementedException();
       }

       public void SetDeviceSchedule(string devid, string segtime, string seglevel)
       {
           if (devid == "*")
           {
               foreach (ICoordinatorDevice coor in Coordinators)
               {
                    
                       coor.SetDeviceSchedule("*", segtime,seglevel);
                    
               }
           }
           else
           {
                
              //   StreetLightInfo[] backInfo =   this.GetStreetLightList(devid);
                   
                   this[devid].SetDeviceSchedule(devid,segtime,seglevel);
                
           }

            
       }


     

       public   async  void  SetDeviceScheduleAsync(string devid, string segtime, string seglevel)
       {
           if (devid == "*")
           {
               foreach (ICoordinatorDevice coor in Coordinators)
               {

                    coor.SetDeviceScheduleAsync("*", segtime, seglevel);

               }
           }
           else
           {

               //StreetLightInfo[] backInfo = await this.GetStreetLightListAsync(devid);
               //await Task.Delay(100);
                 this[devid].SetDeviceScheduleAsync(devid, segtime, seglevel);

           }


       }

       public  async  Task<DeviceInfo[]> GetDeviceListAsync()
       {
           List<DeviceInfo> list = new List<DeviceInfo>();
           foreach (ICoordinatorDevice coor in Coordinators)
           {
               
                   list.AddRange(await coor.GetDeviceListAsync());


                
           }

           return list.ToArray();
       }

       public void SetDeviceScheduleEnable(string devid, bool enable)
       {
           if (devid == "*")
           {
               foreach (ICoordinatorDevice coor in Coordinators)
               {
                   try
                   {
                       coor.SetDeviceScheduleEnable("*", enable);
                   }
                   catch
                   {
                       ;
                   }
               }
           }
           else
           {
               
                   this[devid].SetDeviceScheduleEnable(devid, enable);
              
           }
       }
    }
}
