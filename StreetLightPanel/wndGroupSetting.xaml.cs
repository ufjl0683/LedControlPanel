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
    /// wndGroupSetting.xaml 的互動邏輯
    /// </summary>
    public partial class wndGroupSetting : Window
    {

        public string SelectedGroupName;
        Config conf;
       // StreetLightPanel.StreetLightBindingData[] bindingDatas;
        MainWindow mainwnd;
        public wndGroupSetting(Config config, MainWindow mainwnd)
        {
            InitializeComponent();
            this.mainwnd = mainwnd;
            this.conf = config;
            //this.bindingDatas = bindingDatas;
            
            this.lstGroup.ItemsSource = config.Groups;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lstGroup.SelectedItem == null)
                return;
            conf.Groups.Remove(lstGroup.SelectedItem as Group);
            this.lstGroup.ItemsSource = null;
          
            this.lstGroup.ItemsSource = conf.Groups;
            this.mainwnd.SaveConfig(mainwnd.LedConfig);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (this.lstGroup.SelectedItem == null)
                return;
            SelectedGroupName = (this.lstGroup.SelectedItem as Group).GroupName ;
            this.DialogResult = true;
        
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
