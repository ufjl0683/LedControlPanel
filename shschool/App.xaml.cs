using CeraDevices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace shschool
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        public CeraDevices.CoordinatorDevice dev;
        public StreetLightInfo[] street_light_info;
        public bool IsStart = true;


        
        public App()
        {
              dev = new CeraDevices.CoordinatorDevice("http://10.10.1.1:8080");
            try
            {
#if !DEBUG
              street_light_info=  dev.GetStreetLightList();
#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "," + ex.StackTrace);
                Environment.Exit(-1);
            }
        }
    }
}
