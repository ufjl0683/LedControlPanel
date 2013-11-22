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
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetGroupDim();
          
        }


        public void SetGroupDim()
        {
            foreach (GroupConfig group in App.config.Groups)
            {
                foreach (DeviceConfig dev in group.Devices)
                {

                    new Task(() =>
                    {
                        App.devmgr[dev.RmkID].SetDeviceDimLevel(App.devmgr.GetDeviceID(dev.RmkID), group.DimLevel);
                    }).Start();
                }

            }
        }

     
    }
}
