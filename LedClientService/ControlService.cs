using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlServerCe;
using System.Diagnostics;

namespace LedClientService
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的類別名稱 "ControlService"。
   [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ControlService : IControlService
    {
       string LedDataServiceConnctionString = "http://192.192.85.40/LedDataService/LedDataService.svc";
       string LedImageUrlFormat = "http://192.192.85.40/Led/Images/{0}/{1}";
       System.Collections.Generic.List<ICallBack> callBacks = new List<ICallBack>();
       System.Threading.Timer tmrCheckConnection;


       public ControlService()
       {
           tmrCheckConnection = new System.Threading.Timer(tmrCheckConnection_elapse);
           tmrCheckConnection.Change(60 * 1000, 0);
       }
       void tmrCheckConnection_elapse(object sender)
       {
         
           foreach (ICallBack callback in callBacks.ToArray())
           {
               try
               {
                   callback.ToClientSayHello("hello");
                   
               }
               catch
               {
                   Console.WriteLine("Remove Connection!");
                   callBacks.Remove(callback);
               }
               finally
               {
                   Console.WriteLine("Connection checked!");
                   tmrCheckConnection.Change( 60 * 1000,0);
               }
           }

       }


       public void ImportDevices(string serverIP, int projectID)
       {
           LedClientService.LedLocalDb local = new LedLocalDb();
           LedDataService.DB db = new LedDataService.DB(new Uri("http://" + serverIP + "/LedDataService/LedDataService.svc", UriKind.Absolute));
          
               var query = ((from n in db.tblDevice where n.ProjectID == projectID select n));

              

               foreach (tblDevice tmpdev in local.tblDevice)
                   local.tblDevice.DeleteObject(tmpdev);

               foreach (LedDataService.tblDevice dev in query)
               {
                   local.tblDevice.AddObject(
                         new tblDevice()
                         {
                             ProjectID = dev.ProjectID,
                             ColorType = dev.ColorType,
                             DeviceID = dev.DeviceID,
                             DevType = dev.DevType,
                             GroupID = dev.GroupID,
                             Rotation = dev.Rotation,
                             SectionID = dev.SectionID,
                             Shape = dev.Shape,
                             X = dev.X,
                             Y = dev.Y,
                             ZeeBeeID = dev.ZeeBeeID

                         }
                       );
               }

               local.SaveChanges();
          
       }

       public void ImportProject(string serverIP,int projectid)
        {
            LedDataService.DB db = new LedDataService.DB(new Uri("http://" + serverIP + "/LedDataService/LedDataService.svc", UriKind.Absolute));
           
            var query =( (from n in db.tblProject where n.ProjectID == projectid select n));
       //     db.Execute(prjs);
          
            LedDataService.tblProject prj;
            try
            {
                //project
                prj = query.ToArray<LedDataService.tblProject>().First();
                LedClientService.LedLocalDb local = new LedLocalDb();

                foreach (tblProject tmpprj in local.tblProject)
                    local.tblProject.DeleteObject(tmpprj);

                local.SaveChanges();

                tblProject project = new tblProject()
                {
                     ProjectID=prj.ProjectID,
                      ProjectName=prj.ProjectName
                };

                local.AddTotblProject(project);
                local.SaveChanges();

            //project group
              var  groupquery = from n in db.tblProjectGroup where n.ProjectID == projectid select n;
              foreach (LedDataService.tblProjectGroup prjgrp in groupquery)
              {
                  local.tblProjectGroup.AddObject(
                      new tblProjectGroup()
                      {
                           ProjectID=prjgrp.ProjectID,
                            GroupID=prjgrp.GroupID,
                             GroupName=prjgrp.GroupName,
                              GroupPicture=prjgrp.GroupPicture,
                         
                      }
                      );
              }
              local.SaveChanges();
// projectgroupsection
              var secqry = from n in db.tblProjectGroupSection where n.ProjectID == projectid select n;
              foreach (LedDataService.tblProjectGroupSection sec in secqry)
              {
                  local.tblProjectGroupSection.AddObject(
                      new tblProjectGroupSection()
                      {
                           ProjectID=sec.ProjectID,
                            GroupID=sec.GroupID,
                             SectionID=sec.SectionID,
                              SectionName=sec.SectionName
                               
                      }
                      );
              }

              local.SaveChanges();

                var devqry= from n in db.tblDevice where n.ProjectID==projectid select n;
                  foreach(LedDataService.tblDevice dev in devqry)
                  {
                      local.tblDevice.AddObject(
                          new tblDevice(){
                               ProjectID=dev.ProjectID,
                                ColorType=dev.ColorType,
                                 DeviceID=dev.DeviceID,
                                  DevType=dev.DevType,
                                   GroupID=dev.GroupID,
                                    Rotation=dev.Rotation,
                                     SectionID=dev.SectionID,
                                      Shape=dev.Shape,
                                       X=dev.X,
                                       Y=dev.Y,
                                        ZeeBeeID=dev.ZeeBeeID,
                                        MAC=dev.MAC
                                      
                          });
                  }
                local.SaveChanges();

                if(!System.IO.Directory.Exists("c:\\WebSite\\Images\\"+projectid))
                    System.IO.Directory.CreateDirectory("c:\\WebSite\\Images\\"+projectid);
                foreach (LedClientService.tblProjectGroup grp in local.tblProjectGroup)
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    wc.DownloadFile(string.Format("http://"+serverIP+"/Led/Images/{0}/{1}",grp.ProjectID,grp.GroupPicture),
                       "c:\\WebSite\\Images\\"+projectid+"\\"+grp.GroupPicture );
                }
            }
            catch(Exception ex)
            {
                Util.WriteEvent(ex.Message + "," + ex.StackTrace);
                throw ex;
            }
            #region  db.sdf
            //SqlCeConnection cn = new SqlCeConnection(LedClientService.Properties.Settings.Default.DbConnectionString);
            //try
            //{
            //    cn.Open();
            //    SqlCeCommand cmd = new SqlCeCommand();
            //    cmd.Connection = cn;
            //    cmd.CommandText = "delete from tblProject";
            //    cmd.ExecuteNonQuery();
            //    cmd.CommandText = "delete from [tblProjectGroup]";
            //    cmd.ExecuteNonQuery();
            //    cmd.CommandText = "delete from tblProjectGroupSection";
            //    cmd.ExecuteNonQuery();
            //    cmd.CommandText = "delete from tblDevice";
            //    cmd.ExecuteNonQuery();

            //    cmd.CommandText = string.Format("insert into tblProject (ProjectID,ProjectName) values({0},'{1}')", prj.ProjectID, prj.ProjectName);
            //    cmd.ExecuteNonQuery();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //finally
            //{
            //    cn.Close();
            //}
            #endregion


        }

       public void NotifyClientLedConnectionChanged(int deviceID, bool IsConnected)
       {
           foreach (ICallBack callback in this.callBacks.ToArray())
           {
               try
               {
                   callback.ToClientNotifyConnectionChange(deviceID, IsConnected);
               }
               catch (Exception Ex)
               {
                   callBacks.Remove(callback);
                   Console.WriteLine(Ex.Message + ", clear call back connection" );
               }
           }
       }
       public void NotifyClientLedDisplayChange(int DeviceID, LedClientService.Devices.LedOutputData data)
       {
           foreach (ICallBack callback in this.callBacks.ToArray())
           {
               try
               {
                   callback.ToClientNotifyLedDisplayChange(DeviceID, data);
               }
               catch (Exception Ex)
               {
                   callBacks.Remove(callback);
                   Console.WriteLine(Ex.Message + ", clear call back connection");
               }
           }
       }


        public void ToServerSayHello()
        {
          //  throw new NotImplementedException();
        }


        public void Regist( )
        {
           callBacks.Add( OperationContext.Current.GetCallbackChannel<ICallBack>());
           // throw new NotImplementedException();
        }





        public void ReloadSchedule()
        {
            Service1.MainControl.LoadSchedule();
           // throw new NotImplementedException();
        }


        public Devices.LedOutputData[] GetAllLEDDeviceOutput()
        {
           System.Collections.Generic.List<Devices.LedOutputData> lstDevices=new List<Devices.LedOutputData>();
           Devices.GroupSection[] sections = Service1.MainControl.dict_sections.Values.ToArray<Devices.GroupSection>();
           foreach (Devices.GroupSection sec in sections)
           {
            

               foreach (Devices.Device dev in sec.Devices)
               {
                   if (dev.DeviceType == "LED")
                   {
                       try
                       {
                           lstDevices.Add((dev as LedClientService.Devices.LedDevice).GetCurrentOutput());
                       }
                       catch (Exception ex)
                       {
                           Console.WriteLine(ex.Message + "," + ex.StackTrace);
                       }
                       
                   }
               }
           }

           return lstDevices.ToArray();
           
        }


        public Devices.LedDevice[] GetAllLEDDeviceInfo()
        {
          //  throw new NotImplementedException();
            System.Collections.Generic.List<Devices.LedDevice> lstDevices = new List<Devices.LedDevice>();
            Devices.GroupSection[] sections = Service1.MainControl.dict_sections.Values.ToArray<Devices.GroupSection>();
            foreach (Devices.GroupSection sec in sections)
            {


                foreach (Devices.Device dev in sec.Devices)
                {
                    if (dev.DeviceType == "LED")
                    {
                        try
                        {
                            lstDevices.Add(dev as LedClientService.Devices.LedDevice);
                           // lstDevices.Add((dev as LedClientService.Devices.LedDevice).GetCurrentOutput());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + "," + ex.StackTrace);
                        }
                        
                    }
                }
            }

            return lstDevices.ToArray<LedClientService.Devices.LedDevice>();

        }
    }
}
