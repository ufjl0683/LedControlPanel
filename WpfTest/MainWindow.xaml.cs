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
using System.ComponentModel;

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // this.weatherctl.DataContext = new WeatherBindingData() { win_dir = 45 };

        }

        
    }


    public class WeatherBindingData:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        double _win_dir;
        public double win_dir
        {
            get
            {
                return _win_dir;
            }
            set
            {
                if (value != _win_dir)
                {
                    _win_dir = value;
                    if(this.PropertyChanged!=null)
                        this.PropertyChanged(this,new PropertyChangedEventArgs("win_dir"));
                }
            }
        }
   
    }
}
