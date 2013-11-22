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

namespace wpfPanel
{
    /// <summary>
    /// MainPage.xaml 的互動邏輯
    /// </summary>
    public partial class MainPage : Page
    {
        bool CanHandleEvent = false;
        public MainPage()
        {
            InitializeComponent();
            this.listButton.ItemsSource = (App.config).Groups;
            System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
            tmr.Tick += (s, a) =>
            {
                this.CanHandleEvent = true;
                tmr.Stop();
            };
            tmr.Interval = TimeSpan.FromSeconds(0.5);
            tmr.Start();
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CanHandleEvent)
                this.NavigationService.Navigate(new GroupDevice((sender as Button).DataContext as GroupConfig));
            
        }

        private void listButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

          
        }

        private void listButton_TouchDown(object sender, TouchEventArgs e)
        {
            
           
        }

        private void Button_TouchDown(object sender, TouchEventArgs e)
        {
            if (CanHandleEvent)
                this.NavigationService.Navigate(new GroupDevice((sender as Button).DataContext as GroupConfig)  );
           // this.NavigationService.Navigate(new Uri("/GroupDevice.xaml", UriKind.Relative),(sender as Button).DataContext);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
             
        }
       
    }
}
