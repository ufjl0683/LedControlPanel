using CeraDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpfPanel;

namespace InitialCoor
{
    /// <summary>
    /// Setting.xaml 的互動邏輯
    /// </summary>
    public partial class Setting : Page
    {

        CeraDevices.CoordinatorDevice device;
        System.Windows.Threading.DispatcherTimer tmr;
        public string BaseUrl;
        public Setting(string BaseUrl)
        {
            this.BaseUrl = BaseUrl;
            InitializeComponent();
          

        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            device = new CeraDevices.CoordinatorDevice(BaseUrl);
            try
            {
                StreetLightInfo[] infos = await device.GetStreetLightListAsync();
                this.grdDeviceInfo.ItemsSource = infos;
                tmr=new System.Windows.Threading.DispatcherTimer(){ Interval=TimeSpan.FromSeconds(5)};
                tmr.Tick += tmr_Tick;
                tmr.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


          
        }

      async  void tmr_Tick(object sender, EventArgs e)
        {
            if (chkAutoRefresh.IsChecked == true)
            {
                StreetLightInfo[] infos = await device.GetStreetLightListAsync();
                this.grdDeviceInfo.ItemsSource = infos;
            }
        }

      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StreetLightInfo info = (sender as Button).DataContext as StreetLightInfo;
            device.Kick(info.DevID);

        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private  async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StreetLightInfo[] infos = await device.GetStreetLightListAsync();
            this.grdDeviceInfo.ItemsSource = infos;
         
        }

        private void btnSetDimm_Click(object sender, RoutedEventArgs e)
        {

            //for (int i = 0; i < 100; i++)
            //{
              //  System.Diagnostics.Debug.Print(i.ToString());
                foreach (StreetLightInfo info in this.grdDeviceInfo.ItemsSource)
                {

                    device.SetDeviceDimLevel(info.DevID, (int)slider.Value);
                }
            //}
        }

        DateTime LastChange;
        private void slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            StreetLightInfo[] infos = await device.GetStreetLightListAsync();

            foreach (StreetLightInfo info in infos)
            {
                device.Kick(info.DevID);
            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            StreetLightInfo[] infos = await device.GetStreetLightListAsync();
            Config config = new wpfPanel.Config();
            config.Coordinators = new CoordinatorConfig[]
            {
                new CoordinatorConfig(){ ID=0, BaseUrl=this.BaseUrl}
            };
            config.Groups = new GroupConfig[]{
                new GroupConfig(){ GroupID=0, GroupName="ALL",   DimLevel=0}
            };

          //  config.Groups[0].Devices = new DeviceConfig[infos.Length];

            var q = from n in infos orderby n.RmkID select new DeviceConfig() { CoordinatorID = 0, RmkID = n.RmkID, MAC = n.MAC };
            //for (int i = 0; i < infos.Length; i++)
            //{
            config.Groups[0].Devices = q.ToArray();  // = new DeviceConfig() { CoordinatorID = 0, RmkID = infos[i].RmkID ,MAC=infos[i].MAC};
            //}
            Config.WriteXml(config, "config.xml");
        }

        private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            
        }

        private async void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tb = sender as TextBox;

                //if (tb.Text.Length == 21)
                //{

                    string[] strs = tb.Text.ToLower().Trim().Replace(" ","").Split(new char[] { '-' });
                    if (strs.Length != 2)
                    {
                        MessageBox.Show("format error");
                        return;
                    }
                    string mac = strs[0];
                    string RmkID = strs[1];
                    if (chkOption.IsChecked==true)
                    {
                        device.AddDeviceByMAC(mac);
                        return;
                    }
                    DeviceInfo[] devinfos;//= await device.GetDeviceListAsync();
                    // string devid;
                    DeviceInfo info;
                    DateTime dt = System.DateTime.Now;
                    do
                    {

                        devinfos = await device.GetDeviceListAsync();

                        info = devinfos.Where(n => n != null && n.mac == mac).FirstOrDefault();
                        System.Threading.Thread.Sleep(1000);
                        if (DateTime.Now.Subtract(dt) > TimeSpan.FromSeconds(30))
                        {
                            //  device.SetStreetLightRemark(info.addr, RmkID + mac);
                            MessageBox.Show("time out");
                            tb.Text = "";
                            return;

                        }
                    } while (info == null);

                    device.SetStreetLightRemark(info.addr, RmkID + mac);
                    tb.Text = "";
                }


            }
             
              
        //}
    }
}
