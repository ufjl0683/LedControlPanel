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

namespace shschool
{
    /// <summary>
    /// GroupDevice.xaml 的互動邏輯
    /// </summary>
    public partial class GroupDevice : Page
    {
        //CeraDevices.DeviceManager devmgr = App.devmgr;
        //GroupConfig GroupInfo;
   //     StreetLightBindingData bindingData;
        StreetLightBindingDataGroup bindingDataGroup;
        CeraDevices.CoordinatorDevice ceraDev = new CoordinatorDevice("http://10.10.1.1:8080");
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        bool IsDimmChanage = false;
        public GroupDevice(StreetLightBindingDataGroup group )
        {
            InitializeComponent();
            this.DataContext = bindingDataGroup =group;// this.GroupInfo = GroupInfo;
            group.PropertyChanged+=GroupDevice_PropertyChanged;
          
          
        }

        void GroupDevice_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "DimLevel")
                return;
            try
            {
                IsDimmChanage = true;
                foreach (StreetLightBindingData data in bindingDataGroup.BindingDatas)
                {
                    if (data.IsEnable)
                    {
                        data.DimLevel = bindingDataGroup.DimLevel;

                        ceraDev.SetDeviceDimLevel(data.DevID, data.DimLevel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "," + ex.StackTrace);
            }
            //GroupConfig info = this.DataContext as GroupConfig;
            //foreach (wpfPanel.DeviceConfig config in info.Devices)
            //{
            //    Task task = new Task(() =>
            //    {
            //        devmgr[config.RmkID].SetDeviceDimLevel(devmgr.GetDeviceID(config.RmkID), GroupInfo.DimLevel);
            //        //   System.Diagnostics.Debug.Print(((int)e.NewValue).ToString());
            //    });
            //    task.Start();

            //}
            //throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Config.WriteXml(App.config, "config.xml");
            this.tmr.Stop();
            this.NavigationService.GoBack();
        }
        
        private void Button_TouchDown(object sender, TouchEventArgs e)
        {
         //   e.Handled = true;
         ////   e.RoutedEvent.RoutingStrategy = RoutingStrategy.Direct;
         //   Config.WriteXml(App.config, "config.xml");
            this.tmr.Stop();
            
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
           // this.GroupInfo.PropertyChanged -= GroupDevice_PropertyChanged;
        }

        public async   void BindGroupStreeLightInfo()
        {
          //  DeviceConfig dev = GroupInfo.Devices[0];

         //StreetLightInfo[] infos  = await ( devmgr[dev.RmkID].GetStreetLightListAsync ());
         //string[] keys = (from k in GroupInfo.Devices select k.RmkID).ToArray();

            var a=  (from k in bindingDataGroup.BindingDatas  where k.IsEnable select  k.DevID  ).ToArray();
              StreetLightInfo[] data=null;
            try
            {
                data = await ceraDev.GetStreetLightListAsync();
            }
            catch
            {
               
            }
            if (data != null)
            {
                var q = from n in data where a.Contains(n.DevID)   select n;
                this.grid.ItemsSource = q.ToArray();
            }
           
        }

        private  void Page_Loaded(object sender, RoutedEventArgs e)
        {
         //this.GroupInfo.PropertyChanged += GroupDevice_PropertyChanged;

         //await BindGroupStreeLightInfo();

            BindGroupStreeLightInfo();

            tmr.Interval = TimeSpan.FromSeconds(10);
            tmr.Tick += (s, a) =>
            {
                if(chkAuto.IsChecked==true)
                           BindGroupStreeLightInfo();

                foreach (StreetLightInfo data in grid.ItemsSource)
                {
                    if (data.CurrentDimLevel != (this.DataContext as StreetLightBindingDataGroup).DimLevel  && IsDimmChanage)
                        ceraDev.SetDeviceDimLevelAsync(data.DevID, (this.DataContext as StreetLightBindingDataGroup).DimLevel);
                }
            };

            tmr.Start();
           
        }

        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BindGroupStreeLightInfo();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
