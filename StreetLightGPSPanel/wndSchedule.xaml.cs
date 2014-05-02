using CeraDevices;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace StreetLightPanel
{
    /// <summary>
    /// wndSchedule.xaml 的互動邏輯
    /// </summary>
    public partial class wndSchedule : Window
    {
        string devid;
        CeraDevices.DeviceManager dev_mgr;
        StreetLightInfo info;
        public wndSchedule(string devid)
        {
            InitializeComponent();
           this.Title=  this.devid = devid;
        
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dev_mgr=(App.Current as App).coor_mgr;
            try
            {
                CeraDevices.StreetLightInfo[] infos = dev_mgr.GetStreetLightList(devid);
                datagrid1.ItemsSource = infos[0].sch.Segnments.OrderBy(n => n.Time).ToArray();
                info = infos[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               dev_mgr.SetDeviceSchedule(devid, info.GetScheduleSegTimeString(), info.GetScheduleSegLevelString());
               // dev_mgr.SetDeviceScheduleEnable(devid, true);
               // dev_mgr.SetDeviceRTC(devid, DateTime.Now);
                MessageBox.Show("傳送完成");
            }
            catch { ;}
           
            //this.Close();
        }
    }
}
