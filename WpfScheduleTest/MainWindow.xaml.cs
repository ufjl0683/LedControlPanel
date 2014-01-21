using CeraDevices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfScheduleTest
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        CeraDevices.CoordinatorDevice dev = new CeraDevices.CoordinatorDevice("http://10.10.1.1:8080");
        ScheduleSegnment[] Schedules = new ScheduleSegnment[10];
        
        int cyclecnt=0;
        int cycle = 10;
        public MainWindow()
        {
            InitializeComponent();
#if !DEBUG
            System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
            tmr.Tick += async   (s, a) =>
                {
                    try
                    {
                        StreetLightInfo[] infos=null;
                        if (chkStopCycleQuery.IsChecked == false)
                        {
                            
                            infos = await dev.GetVisibleStreetLightListAsync();
                            
                            this.datagrid1.ItemsSource = infos.OrderBy(n=>n.DevID).ToArray();
                          
                            CeraDevices.Schedule sch = new CeraDevices.Schedule();
                            sch.Segnments = Schedules;
                            int repcnt = 0;
                            foreach (StreetLightInfo info in infos)
                            {
                                if (chkIsRepair.IsChecked == true && !sch.IsEqual(info.sch))
                                {
                                    dev.SetDeviceSchedule(info.DevID, sch.GetScheduleSegTimeString(), sch.GetScheduleSegLevelString());
                                    repcnt++;
                                }
                              //  grdSchedule.ItemsSource
                            }

                            this.Title = repcnt + "/" + infos.Length.ToString();
                            
                        }
                        if (cyclecnt++ * cycle % 600 == 0) //10 min
                            if (chkIsLog.IsChecked == true)
                                savelog(infos);
                       
                    }
                    catch { ;}
                };

            tmr.Interval = TimeSpan.FromSeconds(10);
            tmr.Start();
           

            
            dev.SetDeviceRTC("*", DateTime.Now);
#endif
            for (int i = 0; i < Schedules.Length; i++)
                Schedules[i] = new ScheduleSegnment();
            this.grdSchedule.ItemsSource = Schedules;
        }

       
     
        private void savelog(StreetLightInfo[] infos)
        {
            try
            {

                bool writeheader = false ;
                if (!System.IO.File.Exists("led.csv"))
                    writeheader = true;
                using (StreamWriter wr = System.IO.File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "led.csv"))
                {
                    if (writeheader)
                        wr.WriteLine("TimeStamp,DevID,MAC,cmt,CurrentDimLevel,V,A,F,W,KWHP,KWHN,IsSchedule,LightSensor,Temperature,rtc");
                    
                    foreach (StreetLightInfo info in infos)
                    {
                        wr.Write(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")+",");
                        wr.Write("\t"+info.DevID + ",");
                        wr.Write("\t"+info.MAC + ",");
                    
                        wr.Write("\t"+info.cmt + ",");
                        wr.Write(info.CurrentDimLevel + ",");
                        wr.Write(info.V + ",");
                        wr.Write(info.A + ",");
                        wr.Write(info.F + ",");
                        wr.Write(info.W + ",");
                        wr.Write(info.KWHP + ",");
                        wr.Write(info.KWHN + ",");
                        wr.Write(info.IsScheduleEnable + ",");
                        wr.Write(info.LightSensor + ",");
                        wr.Write(info.Temperature + ",");
                        wr.WriteLine(info.rtc+"");
                       
                     
                    }

                }
            }
            catch { ;}
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
#if !DEBUG
            StreetLightInfo[] infos = await dev.GetVisibleStreetLightListAsync();
           this.datagrid1.ItemsSource = infos.OrderBy(n=>n.DevID);
#endif
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StreetLightInfo[] infos = this.datagrid1.ItemsSource as StreetLightInfo[];
            int[]times= (from n in Schedules select n.Time).ToArray();
            int[] levels=  (from n in Schedules select n.Level).ToArray();
             string timestr,levelstr;
            timestr=times[0].ToString();
            for(int i=1;i<times.Length;i++)
                timestr+=","+times[i];
            levelstr=levels[0].ToString();
              for(int i=1;i<levels.Length;i++)
                levelstr+=","+levels[i];

              dev.SetDeviceRTC("*", DateTime.Now);
             await  Task.Delay(100);
            foreach (StreetLightInfo info in infos)
            {
                dev.SetDeviceScheduleAsync(info.DevID, timestr, levelstr);
                await Task.Delay(200);
               // dev.SetDeviceScheduleEnableAsync(info.DevID, true);
               //await Task.Delay(200);
              
            }
            MessageBox.Show("ok");
        }

        private void lstStreetLight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async  void btnEnableSch_Click(object sender, RoutedEventArgs e)
        {
          //  dev.SetDeviceScheduleEnable("*", true);
            StreetLightInfo[] infos = this.datagrid1.ItemsSource as StreetLightInfo[];
            foreach (StreetLightInfo info in infos)
            {
                dev.SetDeviceScheduleEnableAsync(info.DevID, true);
               await Task.Delay(100);
            }
            MessageBox.Show("ok");
        }

        private async void btnDisable_Click(object sender, RoutedEventArgs e)
        {
           // dev.SetDeviceScheduleEnable("*", false);
            StreetLightInfo[] infos = this.datagrid1.ItemsSource as StreetLightInfo[];
            foreach (StreetLightInfo info in infos)
            {
                dev.SetDeviceScheduleEnableAsync(info.DevID, false);
                await Task.Delay(100);
            }
            MessageBox.Show("ok");
        }

        private void btnKickAll_Click(object sender, RoutedEventArgs e)
        {
            DeviceInfo[] infos = dev.GetDeviceList();
            foreach (DeviceInfo info in infos)
                            dev.Kick(info.addr);

        }

        private void chkIsLog_Checked(object sender, RoutedEventArgs e)
        {
            cyclecnt = 0;
        }

       

    
    }


    public class ITimeSegToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int time = System.Convert.ToInt32(value);
            return string.Format("{0:00}:{1:00}", time / 60, time % 60);
          
         
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Text.RegularExpressions.Regex regx = new System.Text.RegularExpressions.Regex(@"\d{1,2}:\d{1,2}");
            if (!regx.Match(value.ToString()).Success)
                // return value;
                return -1;
            string[] data = value.ToString().Split(new char[] { ':' });
            int hour = System.Convert.ToInt32(data[0]);
            int min = System.Convert.ToInt32(data[1]);
            if (hour >= 24)
                return -1;
            if (min >= 60)
                return -1;
            return hour * 60 + min;

        }
    }

    //public class IScgheduleDimmLevelConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        int level = System.Convert.ToInt32(value);
    //        if(level>100 || level <0)
    //          throw new Ex
    //     //   throw new NotImplementedException();
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //      //  throw new NotImplementedException();
    //    }
    //}

    public class Schedule:INotifyPropertyChanged
    {
        int _time =0;
        int _level=255;
        public int Time
        {
            get
            {
                return _time;
            }
            set
            {
                if (value != _time)
                {
                       //System.Text.RegularExpressions.Regex regx = new System.Text.RegularExpressions.Regex(@"\d{1,2}:\d{1,2}");
                       //if (!regx.Match(value.ToString()).Success)
                       //    throw new ArgumentException("format err");
                    //int hour = value / 60;
                    //int min = value % 60;
                    if (value==-1)
                       throw new ArgumentException("format err");
                       _time = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Time"));
                 

                }
                 
            }
        }
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value != _level)
                {
                    int res;

                    //if(!int.TryParse(value.ToString(),out res))
                    //    throw new Exception("format err");
                    //if(!(value>=0 && value<=100 || value==255))
                    //    throw new Exception("format err");

                    _level = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Level"));
                         

                }

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
