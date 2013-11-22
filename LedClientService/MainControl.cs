using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LedClientService.Devices;

namespace LedClientService
{
    public   class MainControl
    {
       
        public  System.Collections.Generic.Dictionary<int, GroupSection> dict_sections;
        //LedClientService.tblProjectGroupSection[] sections;
        public CeraDevices.CeraDevice outDev;
        public System.Collections.Generic.Dictionary<ushort,Devices.Device> devices=new Dictionary<ushort,Device>();
       // Schedule.Scheduler scheduler = new Schedule.Scheduler();
        System.Timers.Timer tmr1min = new System.Timers.Timer(60 * 1000);
        public MainControl()
        {


            outDev = new CeraDevices.CeraDevice("Com6", 115200);
         
            LoadDevice();
          
            outDev.OnChildTableReport += new CeraDevices.ChildTableReportHandler(outDev_OnChildTableReport);
            outDev.SendPackage(new CeraDevices.Packages.PermitNoteJoinPackage(30));
            outDev.SendPackage(new CeraDevices.Packages.RequestChildrenTable());
           // System.Threading.Thread.Sleep(5 * 1000);

            new System.Threading.Thread(LoadScheduleTask).Start();

            tmr1min.Elapsed += new System.Timers.ElapsedEventHandler(tmr1min_Elapsed);
           // LoadSchedule();
            tmr1min.Start();
           
        }

        void tmr1min_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("==============Cycle   Output Begin!====================");
           
            foreach (Device dev in devices.Values.ToArray())
            {
                if (dev is LedDevice)
                    (dev as LedDevice).Output();
            }
            //throw new NotImplementedException();
        }

        void outDev_OnChildTableReport(CeraDevices.Packages.ResponseChildTable childTable)
        {  
            //Node Join
            if (devices.ContainsKey(childTable.Dest))
            {
                if (devices[childTable.Dest] is LedDevice)
                    (devices[childTable.Dest] as LedDevice).Output();
            }
           
        }
        DateTime TodayTime(DateTime dt)
        {
            return new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,dt.Hour,dt.Minute,0);
        }
        void LoadScheduleTask()
        {
            System.Threading.Thread.Sleep(1000);
            LoadSchedule();
        }
        public void LoadSchedule()
        {
            LedClientService.LedLocalDb db = new LedLocalDb();
            Schedule.Scheduler.RemoveAll();
            #region Load Daily
            var q= from n in db.tblProjectGroupSection   select n ;
         
            foreach (tblProjectGroupSection section in q)
            {
                bool bFirstFlag = false;
                tblSectionLedPlan lastSectionPlan = null;
                var q1 = from n in section.tblSectionLedPlan orderby n.BeginTime descending select n;
                foreach (tblSectionLedPlan plan in q1)
                {
                    DateTime begtime = TodayTime(plan.BeginTime);
                    if (begtime <= DateTime.Now)
                    {
                        begtime = begtime.AddDays(1);
                        if (!bFirstFlag)
                        {
                            if (lastSectionPlan != null)
                            {
                                LedClientService.Schedule.OneTimeSchedule oneSchedule = new Schedule.OneTimeSchedule("tmp_" + plan.PlanID, DateTime.Now,
                                   (int)TodayTime(lastSectionPlan.BeginTime).Subtract(DateTime.Now).TotalMinutes+1,
                                   new Schedule.ScheduleJob[] { new Schedule.ScheduleJob(plan.SectionID, 0, 
                                   new Devices.LedOutputData(){ SectionID=plan.SectionID, B=plan.B,R=plan.R,G=plan.G,W=plan.W,Priority= OutputPriority.Scheduled}) },
                                   true);
                                Schedule.Scheduler.AddSchedule(oneSchedule);
                            }
                            else
                            {
                                tblSectionLedPlan[] tmp = q1.ToArray<tblSectionLedPlan>();
                                tblSectionLedPlan refplan = tmp[0];
                                int dueMin = (int)TodayTime(refplan.BeginTime).AddDays(1).Subtract(DateTime.Now).TotalMinutes+1;
                                LedClientService.Schedule.OneTimeSchedule oneSchedule = new Schedule.OneTimeSchedule("tmp_" + plan.PlanID, DateTime.Now, dueMin,
                                new Schedule.ScheduleJob[] { new Schedule.ScheduleJob(plan.SectionID, 0, 
                               new Devices.LedOutputData(){SectionID=plan.SectionID, B=refplan.B,W=refplan.W, G=refplan.G, R=refplan.R, Priority= OutputPriority.Scheduled}   ) }
                                , true);
                                Schedule.Scheduler.AddSchedule(oneSchedule);
                            }
                            bFirstFlag = true;
                        }
                    }
                    else
                        lastSectionPlan = plan;


                    Schedule.Schedule schedule = new Schedule.DailySchedule("Daily" + plan.PlanID, begtime,
                       0, new Schedule.ScheduleJob[] { new Schedule.ScheduleJob(plan.SectionID, 0,
                           new Devices.LedOutputData(){ SectionID=plan.SectionID, R=plan.R,G=plan.G,B=plan.B,W=plan.W,Priority= OutputPriority.Scheduled})  }, true); ;

                    Schedule.Scheduler.AddSchedule(schedule);
                    Console.WriteLine("Dialy Schedule" + plan.PlanID + " Loaded!");

                    //for (int i = 0; i < Schedule.Scheduler.Count(); i++)
                    //{
                    //    Schedule.Scheduler.GetScheduleAt(i).ToString();
                    //}
                }
            }


            #endregion
            #region Load OneTime
             var qq = from n in db.tblSectionLedOneTimePlan  where n.BeginTime > DateTime.Now  select n;
            foreach (tblSectionLedOneTimePlan plan in qq)
            {
                DateTime begtime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, plan.BeginTime.Hour, plan.BeginTime.Minute, 0);
                if (begtime <= DateTime.Now)
                    continue;

                Schedule.Schedule schedule = new Schedule.OneTimeSchedule("OneTime" + plan.PlanID, begtime,
                   plan.DurationMin, new Schedule.ScheduleJob[] { new Schedule.ScheduleJob(plan.SectionID, 0, 
                   new Devices.LedOutputData(){SectionID=plan.SectionID, W=plan.W,R=plan.R,G=plan.G,B=plan.B,Priority= OutputPriority.OneTime}  ) }, true); ;

                Schedule.Scheduler.AddSchedule(schedule);
                Console.WriteLine("One Time Schedule" + plan.PlanID + " Loaded!");

                //for (int i = 0; i < Schedule.Scheduler.Count(); i++)
                //{
                //  Console.WriteLine(  Schedule.Scheduler.GetScheduleAt(i).ToString());
                //}
            }
            #endregion

        }

        void LoadDevice()
        {
           LedLocalDb db = new LedLocalDb();
            dict_sections = new Dictionary<int, GroupSection>();
            var q = from n in db.tblProjectGroupSection select new GroupSection() { SectionID = n.SectionID, SectionName = n.SectionName };
            foreach (GroupSection sec in q)
            {
                dict_sections.Add(sec.SectionID, sec);
                Console.WriteLine("section:" + sec.SectionName + ",loading!");

                var qq = from n in db.tblDevice where n.SectionID == sec.SectionID select n;

                foreach (tblDevice device in qq)
                {
                    if (device.DevType == "LED")
                    {
                        LedDevice leddev=new LedDevice()
                        {   SectionID=device.SectionID,
                            DeviceType = device.DevType,
                            DeviceID = device.DeviceID,
                            LedType = device.Shape,
                            ZeeBeeID=(int)device.ZeeBeeID
                        };
                        sec.Devices.Add((Device)leddev);
                        leddev.OnConnectChanged += new OnConnectedChangeHandler(Device_OnConnectChanged);
                        devices.Add((ushort)leddev.ZeeBeeID, leddev);
                    }
                    Console.WriteLine("\tDevice ID " + device.DeviceID + ",loading");
                }
            }
        }

        void Device_OnConnectChanged(Device dev)
        {

            Service1.ControlService.NotifyClientLedConnectionChanged(dev.ZeeBeeID, dev.IsConnected);
            //throw new NotImplementedException();
        }
    }
}
