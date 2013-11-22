using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace slPanel
{
    public partial class chldLedPlanEdit : ChildWindow
    {
        slPanel.Web.DomainService1 dbservice;
        public chldLedPlanEdit(slPanel.Web.DomainService1 dbservice)
        {
            InitializeComponent();
            this.dbservice = dbservice;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Web.tblProjectGroupSection sec = (this.DataContext as slPanel.Web.tblProjectGroupSection);
            
           //    slPanel.Web.DomainService1 sev=new Web.DomainService1();
               
            this.DialogResult = false;
        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            Web.tblProjectGroupSection sec = this.DataContext as slPanel.Web.tblProjectGroupSection;
            sec.tblSectionLedPlan.Add(new Web.tblSectionLedPlan() {BeginTime=new DateTime(1900,1,1,0,0,0), W = 255, R = 255, G = 255, B = 255 });
        
        }

        private void hyperlinkButton2_Click(object sender, RoutedEventArgs e)
        {
            Web.tblProjectGroupSection sec = this.DataContext as slPanel.Web.tblProjectGroupSection;
            if (this.dataGrid1.SelectedItem == null)
            {
                MessageBox.Show("請先選取資料");
                return;
            }
            slPanel.Web.tblSectionLedPlan plan = dataGrid1.SelectedItem as slPanel.Web.tblSectionLedPlan;
            if (MessageBox.Show("確定要刪除", "slPanel", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            this.dbservice.tblSectionLedPlans.Remove(plan);

        }

        private void hyperlinkButton3_Click(object sender, RoutedEventArgs e)
        {
            Web.tblProjectGroupSection sec = this.DataContext as slPanel.Web.tblProjectGroupSection;
            sec.tblSectionLedOneTimePlan.Add(new Web.tblSectionLedOneTimePlan() { BeginTime = new DateTime(1900, 1, 1, 0, 0, 0), DurationMin=60,W = 255, R = 255, G = 255, B = 255 });
        }

        private void hyperlinkButton4_Click(object sender, RoutedEventArgs e)
        {
            Web.tblProjectGroupSection sec = this.DataContext as slPanel.Web.tblProjectGroupSection;
            if (this.dataGrid2.SelectedItem == null)
            {
                MessageBox.Show("請先選取資料");
                return;
            }
            slPanel.Web.tblSectionLedOneTimePlan plan = dataGrid2.SelectedItem as slPanel.Web.tblSectionLedOneTimePlan;
            if (MessageBox.Show("確定要刪除", "slPanel", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            this.dbservice.tblSectionLedOneTimePlans.Remove(plan);
        }
    }
}

