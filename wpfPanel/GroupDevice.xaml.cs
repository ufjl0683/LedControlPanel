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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfPanel
{
    /// <summary>
    /// GroupDevice.xaml 的互動邏輯
    /// </summary>
    public partial class GroupDevice : Page
    {
        CeraDevices.DeviceManager devmgr = App.devmgr;
        GroupConfig GroupInfo;
        
        public GroupDevice(GroupConfig GroupInfo)
        {
            InitializeComponent();
           this.DataContext=  this.GroupInfo = GroupInfo;
         
          
          
        }

        void GroupDevice_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName!="DimLevel")
                return;
            GroupConfig info = this.DataContext as GroupConfig;
            foreach (wpfPanel.DeviceConfig config in info.Devices)
            {
                Task task = new Task(() =>
                {
                    devmgr[config.RmkID].SetDeviceDimLevel(devmgr.GetDeviceID(config.RmkID),GroupInfo.DimLevel);
                    //   System.Diagnostics.Debug.Print(((int)e.NewValue).ToString());
                });
                task.Start();

            }
            //throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Config.WriteXml(App.config, "config.xml");
            this.NavigationService.GoBack();
        }
        
        private void Button_TouchDown(object sender, TouchEventArgs e)
        {
            e.Handled = true;
         //   e.RoutedEvent.RoutingStrategy = RoutingStrategy.Direct;
            Config.WriteXml(App.config, "config.xml");
            this.NavigationService.GoBack();
        }

        private    void DimmAdjust_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           
        }

        private void DimmAdjust_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
           
        }
        
        private void DimmAdjust_TouchUp(object sender, TouchEventArgs e)
        {
            
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.GroupInfo.PropertyChanged -= GroupDevice_PropertyChanged;
        }

        public async   Task BindGroupStreeLightInfo()
        {
            DeviceConfig dev = GroupInfo.Devices[0];

         StreetLightInfo[] infos  = await ( devmgr[dev.RmkID].GetStreetLightListAsync ());
         string[] keys = (from k in GroupInfo.Devices select k.RmkID).ToArray();
              this.grid.ItemsSource= infos.Where(n=>keys.Contains(n.RmkID));
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
         this.GroupInfo.PropertyChanged += GroupDevice_PropertyChanged;

         await BindGroupStreeLightInfo();
         System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
          
         tmr.Interval = TimeSpan.FromSeconds(5 );
         tmr.Tick += (s, a) =>
         {

             BindGroupStreeLightInfo();
         };

         tmr.Start();
           
        }

       
    }
}
