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
using System.Windows.Navigation;
using System.ServiceModel.DomainServices.Client;

namespace slPanel
{
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
        //    this.NavigationService.Navigate(new Uri("/Panel.xaml?UserID=" + "david", UriKind.Relative));
        //    return;
            slPanel.Web.DomainService1 db = new Web.DomainService1();

            EntityQuery<slPanel.Web.tblUser> q = from n in db.GetTblUserQuery() where n.UserID == txtUserID.Text.Trim() && n.Passwoord == txtPassword.Password.Trim() select n;

            LoadOperation lo = db.Load(q);
            lo.Completed += (s, a) =>
            {
                if (lo.Error != null)
                {
                    MessageBox.Show(lo.Error.Message);
                    return;
                }
                if (lo.Entities.Count() != 0)
                    this.NavigationService.Navigate(new Uri("/Panel.xaml?UserID="+txtUserID.Text, UriKind.Relative));
                else
                    MessageBox.Show("帳號或密碼錯誤!");


            };




        }

    }
}
