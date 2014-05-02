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
    /// wndTempleateEdit.xaml 的互動邏輯
    /// </summary>
    public partial class wndTempleateEdit : Window
    {
        System.Collections.Generic.List<Scenarior> Scenariors;

        public Scenarior SelectedScenarior { get; set; }
        public wndTempleateEdit(System.Collections.Generic.List<Scenarior> Scenariors)
        {
            InitializeComponent();
           this.lstScenarioName.ItemsSource =  this.Scenariors=Scenariors;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
          string sceneName=  Microsoft.VisualBasic.Interaction.InputBox("請輸入範本名稱", "StreetLight");
          if (sceneName == "")
              return;

            CeraDevices.ScheduleSegnment[] segs=new CeraDevices.ScheduleSegnment[10];
            for(int i=0;i<segs.Length;i++)
                segs[i]=new CeraDevices.ScheduleSegnment(){ Time=0,Level=255};
            Scenariors.Add(new Scenarior() { SceneName = sceneName, Schedule = new CeraDevices.Schedule() { Segnments = segs } });
            this.lstScenarioName.ItemsSource = null;
            this.lstScenarioName.ItemsSource = Scenariors;
        }

        

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.lstScenarioName.SelectedItem != null)
                this.Scenariors.Remove(this.lstScenarioName.SelectedItem as Scenarior);
            this.lstScenarioName.ItemsSource = null;
            this.lstScenarioName.ItemsSource = Scenariors;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {

            if (this.lstScenarioName.SelectedItem != null)
            {
                this.DialogResult = true;
                this.SelectedScenarior = lstScenarioName.SelectedItem as Scenarior;
                this.Close();
            }
            else
                MessageBox.Show("必須選取範本!");
        }


    }
}
