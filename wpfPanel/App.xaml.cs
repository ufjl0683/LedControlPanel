using CeraDevices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace wpfPanel
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        public static CeraDevices.DeviceManager devmgr;
        public static Config config;
        static App()
        {
             config = Config.ReadXml("config.xml");
            CeraDevices.CoordinatorDevice[] devices = new CoordinatorDevice[config.Coordinators.Length];
            for (int i = 0; i < config.Coordinators.Length; i++)
                devices[i] = new CoordinatorDevice(config.Coordinators[i].BaseUrl);
            devmgr = new DeviceManager(devices);

        }

     //   public Config Config;
        public App()
        {
            //Config = Config.ReadXml("config.xml");
        }

    }
}
