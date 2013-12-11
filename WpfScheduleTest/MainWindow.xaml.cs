using CeraDevices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        Schedule[] Schedules = new Schedule[10];
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
            tmr.Tick += (s, a) =>
                {
                    try
                    {
                        lstStreetLight.ItemsSource = dev.GetVisibleStreetLightList();
                    }
                    catch { ;}
                };

            tmr.Interval = TimeSpan.FromSeconds(5);
            tmr.Start();
            for (int i =0 ;i<Schedules.Length;i++)
               Schedules[i] = new Schedule();

            this.grdSchedule.ItemsSource = Schedules;
            dev.SetDeviceRTC("*", DateTime.Now);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StreetLightInfo[] infos = dev.GetVisibleStreetLightList();
            this.lstStreetLight.ItemsSource = infos;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StreetLightInfo[] infos = lstStreetLight.ItemsSource as StreetLightInfo[];
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
            foreach (StreetLightInfo info in infos)
            {
                dev.SetDeviceSchedule(info.DevID, timestr, levelstr);
                dev.SetDeviceScheduleEnable(info.DevID, true);
            }
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
