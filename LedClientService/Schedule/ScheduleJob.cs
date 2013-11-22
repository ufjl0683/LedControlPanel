﻿using System;
using System.Collections.Generic;
using System.Text;
using LedClientService.Devices;


namespace LedClientService.Schedule
{
  public   class ScheduleJob
    {
      public  int SectionID;
      public object jobObj;
      public int jobId,schid;
    //  Schedule sch;
      bool IsSuccess = false;
      Schedule schedule;
      System.Data.DataSet  orgds;
      string errmesg="";
      public ScheduleJob(/*int schid,*/int SectionID,int jobId, object jobObj)
      {
          this.SectionID = SectionID;
          this.jobObj = jobObj;
          this.jobId = jobId;
         // this.schid = schid;
      }

      public  void setSchedule(Schedule sch)
      {
          this.schedule=sch;
      }
      public void DoJob()
      {
         // foreach(object job in this.jobObj)
          if (this.jobObj is LedClientService.Devices.LedOutputData)
          {
              LedOutputData outdata=this.jobObj as LedOutputData;
             
               if(outdata==null) return;
               foreach (Device dev in Service1.MainControl.dict_sections[outdata.SectionID].Devices)
               {
                   try
                   {
                       if (dev is LedDevice)
                       {
                           ((LedDevice)dev).SetOutput(outdata);
                       }
                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ex.Message + "," + ex.StackTrace);
                   }
               }

          }

          //if (this.jobObj == null)
          //{
          //    errmesg = "null job";
          //    return;
          //}

          //try
          //{
          //    if (this.jobObj is RemoteInterface.MFCC.RGS_GenericDisplay_Data)
          //        DoRGS_Job(jobObj);
             
          //    else if (this.jobObj is RemoteInterface.HC.ScheduleTask.VDRealTime || this.jobObj is RemoteInterface.HC.ScheduleTask.VDTrigger)
          //        DoVD_Job(jobObj);
          //    else if (this.jobObj is RemoteInterface.HC.ScheduleTask.ReportScheduleTask)
          //        DoReport_Job(jobObj);
          //    else if (this.jobObj is System.Data.DataSet)
          //        DoDataSetJob((System.Data.DataSet)this.jobObj);

          //    else if (this.jobObj is RemoteInterface.HC.ScheduleTask.SetTravelModeTask)
          //        DoSetTravelMode_Job(this.jobObj as RemoteInterface.HC.ScheduleTask.SetTravelModeTask);
          //    else  // other device
          //    {
          //        if(schedule.outputMode == OutputModeEnum.ManualMode)
          //           ((OutPutDeviceBase)Program.matrix.getDeviceWrapper(this.devName)).SetOutput(new OutputQueueData(this.devName, OutputModeEnum.ManualMode, OutputQueueData.NORMAL_MANUAL_RULE_ID, OutputQueueData.NORMAL_MANUAL_PRIORITY, jobObj));
          //        else
          //           ((OutPutDeviceBase)Program.matrix.getDeviceWrapper(this.devName)).SetOutput(new OutputQueueData(this.devName, OutputModeEnum.ScheduleMode, OutputQueueData.SCHEDULE_RULE_ID, OutputQueueData.SCHEDULE_PRIORITY, jobObj));
              
          //    }


          //    IsSuccess = true;
          //    if(this.schedule.m_durationMin==0)
          //          this.FinalJob();
          //}
          //catch (Exception ex)
          //{
          //    errmesg = ex.Message;
          //    IsSuccess = false;
          //}
      }

      /*
      private void  DoSetTravelMode_Job(RemoteInterface.HC.ScheduleTask.SetTravelModeTask job)
      {

          string sql = "";
          if (job.Enable)
          {
              sql = "update tblRGSTravelTime set enable='Y' where deviceName='" + this.devName + "'";
              Program.matrix.dbServer.SendCmdImmediately(sql);

              object dev = Program.matrix.getDevicemanager()[this.devName];
              if (dev is Host.TC.RGSDeviceWrapper)
                  (dev as Host.TC.RGSDeviceWrapper).loadTravelSetting();
              if (dev is Host.TC.CMSDeviceWrapper)
                  (dev as Host.TC.CMSDeviceWrapper).loadTravelSetting();
          }
          else
          {
              sql = "update tblRGSTravelTime set enable='N' where deviceName='" + this.devName + "'";
              object dev = Program.matrix.getDevicemanager()[this.devName];
              Program.matrix.dbServer.SendCmdImmediately(sql);
              if (dev is Host.TC.RGSDeviceWrapper)
                  (dev as Host.TC.RGSDeviceWrapper).removeOutput(OutputQueueData.TRAVEL_PRIORITY);
              if (dev is Host.TC.CMSDeviceWrapper)
                  (dev as Host.TC.CMSDeviceWrapper).removeOutput(OutputQueueData.TRAVEL_PRIORITY);
          }
      }
      private void DoDataSetJob(System.Data.DataSet ds)
      {
         // throw new Exception("The method or operation is not implemented.");
          RemoteInterface.MFCC.I_MFCC_Base robj = null;
          try
          {
              robj = (RemoteInterface.MFCC.I_MFCC_Base)Program.matrix.getRemoteObject(this.devName);
              if (robj == null)
              {
                  errmesg = "robj is null";
                  throw new Exception();

              }


          }
          catch
          {
              throw new Exception(this.devName + "查無對應的MFCC!");
          }

          if (!robj.getConnectionStatus(this.devName))
              throw new Exception(this.devName + "斷線!");

          robj.sendTC(this.devName, ds);
      }


      private void DoReport_Job(object obj)
      {
          RemoteInterface.HC.ScheduleTask.ReportScheduleTask jobObj = (RemoteInterface.HC.ScheduleTask.ReportScheduleTask)obj;

          Program.matrix.WebService.printReport(jobObj.reportId);
      }

      private void DoVD_Job(object obj)
      {
            RemoteInterface.MFCC.I_MFCC_VD robj =null;
          try
          {
              robj = (RemoteInterface.MFCC.I_MFCC_VD)Program.matrix.getRemoteObject(this.devName);
              if (robj == null)
              {
                  errmesg = "robj is null";
                  throw new Exception();
                 
              }

             
          }
          catch 
          {
              throw new Exception(this.devName + "查無對應的MFCC!");
          }

          if(!robj.getConnectionStatus(this.devName))
              throw new Exception(this.devName+"斷線!");

          if (obj is RemoteInterface.HC.ScheduleTask.VDRealTime)
          {
              RemoteInterface.HC.ScheduleTask.VDRealTime jobObj = (RemoteInterface.HC.ScheduleTask.VDRealTime)obj;
              for (int i = 0; i < jobObj.laneids.Length; i++)
                  robj.setRealTime(this.devName,jobObj.laneids[i], jobObj.cycle, jobObj.durationMin);

          }
          else if (obj is RemoteInterface.HC.ScheduleTask.VDTrigger)
          {
              System.Data.DataSet ds=Program.ScriptMgr["VD"].GetSendDataSet("get_trig_config");
              ds.AcceptChanges();
              orgds = Program.matrix.getDeviceWrapper(this.devName).getRemoteObj().sendTC(this.devName, ds);

              ds = Program.ScriptMgr["VD"].GetSendDataSet("set_trig_config");

              RemoteInterface.HC.ScheduleTask.VDTrigger jobObj = (RemoteInterface.HC.ScheduleTask.VDTrigger)obj;
              ds.Tables[0].Rows[0]["lane_count"] = jobObj.landids.Length;
              for (int i = 0; i < jobObj.landids.Length; i++)
              {
                 // System.Data.DataRow r = ds.Tables[1].NewRow();
                 // r["lane_id"] = jobObj.landids[i];
                 // r["occ_time_limit"] = jobObj.occ_time_limit;
                  ds.Tables[1].Rows.Add(jobObj.landids[i], jobObj.occ_time_limit[i]);


              }
              ds.AcceptChanges();

              robj.sendTC(this.devName, ds);
                ;

          }

         
               
      }
      private void DoRGS_Job(object obj)
      {
          //try
          //{

          if(this.schedule.outputMode== OutputModeEnum.ScheduleMode  )
              ((RGSDeviceWrapper)Program.matrix.getDeviceWrapper(this.devName)).SetOutput(new OutputQueueData(this.devName,OutputModeEnum.ScheduleMode, OutputQueueData.SCHEDULE_RULE_ID, OutputQueueData.SCHEDULE_PRIORITY, obj));
          else
              ((RGSDeviceWrapper)Program.matrix.getDeviceWrapper(this.devName)).SetOutput(new OutputQueueData(this.devName, OutputModeEnum.ManualMode, OutputQueueData.NORMAL_MANUAL_RULE_ID, OutputQueueData.NORMAL_MANUAL_PRIORITY, obj));

          //}
          //catch (Exception ex)
          //{
          //    RemoteInterface.ConsoleServer.WriteLine("subschid:" + this.jobId + " " + ex.Message);
          //}

      }

      //private void DoCMS_Job(object obj)
      //{
      //    //try{

      //        RemoteInterface.ConsoleServer.WriteLine("=================output Sched"+ obj.ToString()+"===================");
      //        ((CMSDeviceWrapper)Program.matrix.getDeviceWrapper(this.devName)).SetOutput(new OutputQueueData(OutputModeEnum.ScheduleMode, OutputQueueData.SCHEDULE_RULE_ID, OutputQueueData.SCHEDULE_PRIORITY, obj));
      //    //}
      //    //catch (Exception ex)
      //    //{
      //    //    RemoteInterface.ConsoleServer.WriteLine("subschid:" + this.jobId + " " + ex.Message);
      //    //}
      //}
       *   * */
      public void FinalJob()
      {

          //if (this.schedule.m_durationMin == 0)
          //    return;
          if (this.jobObj is LedOutputData)
          {
              if (this.jobObj != null && (this.jobObj as LedOutputData).Priority == OutputPriority.Scheduled)
                  return;
              
                  LedClientService.Devices.LedOutputData outdata = this.jobObj as LedOutputData;
                  if (outdata == null)
                      return;

                  foreach (Device dev in Service1.MainControl.dict_sections[outdata.SectionID].Devices)
                  {
                      try
                      {
                          if (dev is LedDevice)
                          {
                              ((LedDevice)dev).RemoveOutput(outdata);
                          }
                      }
                      catch (Exception ex)
                      {
                          Console.WriteLine(ex.Message + "," + ex.StackTrace);
                      }
                  }

              
          }


          #region Mark Old
          // try
                 //  {
              
                 ////  if (this.jobObj is RemoteInterface.MFCC.RGS_GenericDisplay_Data || this.jobObj is RemoteInterface.HC.CMSOutputData)
                     
                 //  if(!(this.jobObj is RemoteInterface.HC.ScheduleTask.ReportScheduleTask))

                 //     if (Program.matrix.getDeviceWrapper(devName) is OutPutDeviceBase)
                 //      {
                 //          try
                 //          {
                 //              if(this.schedule.outputMode== OutputModeEnum.ScheduleMode)
                 //              ((OutPutDeviceBase)Program.matrix.getDeviceWrapper(devName)).removeOutput(OutputQueueData.SCHEDULE_RULE_ID);
                 //              else
                 //              ((OutPutDeviceBase)Program.matrix.getDeviceWrapper(devName)).removeOutput(OutputQueueData.NORMAL_MANUAL_RULE_ID);

                 //          }
                 //          catch (Exception ex)
                 //          {
                 //              RemoteInterface.ConsoleServer.WriteLine(ex.Message + "," + ex.StackTrace);
                 //              RemoteInterface.Util.SysLog("schd.log", ex.Message + "," + ex.StackTrace);
                 //          }

                 //      }

                 //      if (this.jobObj is RemoteInterface.HC.ScheduleTask.VDTrigger)
                 //      {
                 //          System.Data.DataSet ds = Program.ScriptMgr["VD"].GetSendDataSet("set_trig_config");
                 //          ds.Tables[0].Rows[0]["lane_count"] = orgds.Tables[0].Rows[0]["lane_count"];
                 //          ds.Tables[1].Merge(orgds.Tables[1]);
                 //          ds.AcceptChanges();
                 //          Program.matrix.getDeviceWrapper(this.devName).getRemoteObj().sendTC(this.devName, ds);
                 //      }

                 //      if (this.jobObj is RemoteInterface.HC.ScheduleTask.SetTravelModeTask)
                 //      {
                 //          string sql = "";
                 //          if ((this.jobObj as RemoteInterface.HC.ScheduleTask.SetTravelModeTask).Enable)
                 //          {

                 //                 sql = "update tblRGSTravelTime set enable='N' where deviceName='" + this.devName + "'";
                 //                      Program.matrix.dbServer.SendCmdImmediately(sql);

                             
                 //          }
                 //          else
                 //          {

                 //              sql = "update tblRGSTravelTime set enable='Y' where deviceName='" + this.devName+ "'";
                 //              Program.matrix.dbServer.SendCmdImmediately(sql);

                 //              //object dev = Program.matrix.getDevicemanager()[job.DevName];
                 //              //if (dev is Host.TC.RGSDeviceWrapper)
                 //              //{
                 //              //    (dev as Host.TC.RGSDeviceWrapper).removeOutput(OutputQueueData.TRAVEL_PRIORITY);
                 //              //    (dev as Host.TC.RGSDeviceWrapper).
                 //              //}
                 //              //if (dev is Host.TC.CMSDeviceWrapper)
                 //              //{
                 //              //    (dev as Host.TC.CMSDeviceWrapper).removeOutput(OutputQueueData.TRAVEL_PRIORITY);
                 //              //}
                 //          }

                 //          object dev = Program.matrix.getDevicemanager()[this.devName];
                 //          if (dev is Host.TC.RGSDeviceWrapper)
                 //          {
                 //              (dev as Host.TC.RGSDeviceWrapper).removeOutput(OutputQueueData.TRAVEL_PRIORITY);
                 //              (dev as Host.TC.RGSDeviceWrapper).loadTravelSetting();
                 //              (dev as Host.TC.RGSDeviceWrapper).DisplayTravelTime();
                 //          }
                 //          if (dev is Host.TC.CMSDeviceWrapper)
                 //          {
                 //              (dev as Host.TC.CMSDeviceWrapper).removeOutput(OutputQueueData.TRAVEL_PRIORITY);
                 //              (dev as Host.TC.CMSDeviceWrapper).loadTravelSetting();
                 //              (dev as Host.TC.CMSDeviceWrapper).DisplayTravelTime();

                 //          }


                 //      }

                      

                 //  }
                 //  catch (Exception ex)
                 //  {
                       
                 //      RemoteInterface.ConsoleServer.WriteLine(ex.Message);
                 //      errmesg = ex.Message + "," + ex.StackTrace;
                 //  }    



                 //Program.matrix.dbServer.SendSqlCmd(string.Format("insert into tblSchLog (schid,subschid,timestamp,result,memo) values({0},{1},'{2}',{3},'{4}')",
                 //    schid,jobId,RemoteInterface.DbCmdServer.getTimeStampString(DateTime.Now),IsSuccess?1:0,IsSuccess?"":errmesg));
                 //errmesg = "";



          //     IsSuccess = false;
          #endregion
      }
     
    }
}

