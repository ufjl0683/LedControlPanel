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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StreetLightPanel.MapControls
{
    /// <summary>
    /// WeatherControl.xaml 的互動邏輯
    /// </summary>
    public partial class WeatherControl : UserControl
    {

        public static readonly DependencyProperty  WindDirectionProperty =
DependencyProperty.Register("WindDirection", typeof(double), typeof(WeatherControl),new FrameworkPropertyMetadata(0.0,
  new PropertyChangedCallback(OnValueChanged)));
        public static readonly DependencyProperty WindSpeedProperty =
DependencyProperty.Register("WindSpeed", typeof(double), typeof(WeatherControl), new FrameworkPropertyMetadata(0.0,
new PropertyChangedCallback(OnValueChanged)));

        public static readonly DependencyProperty TemperatureProperty =
DependencyProperty.Register("Temperature", typeof(double), typeof(WeatherControl), new FrameworkPropertyMetadata(0.0,
new PropertyChangedCallback(OnValueChanged)));

        public static readonly DependencyProperty HumidityProperty =
DependencyProperty.Register("Humidity", typeof(double), typeof(WeatherControl), new FrameworkPropertyMetadata(0.0,
new PropertyChangedCallback(OnValueChanged)));
        public static readonly DependencyProperty PM25Property =
DependencyProperty.Register("PM25", typeof(double), typeof(WeatherControl), new FrameworkPropertyMetadata(0.0,
new PropertyChangedCallback(OnValueChanged)));

        public static readonly DependencyProperty WaterLevelProperty =
DependencyProperty.Register("WaterLevel", typeof(double), typeof(WeatherControl), new FrameworkPropertyMetadata(0.0,
new PropertyChangedCallback(OnValueChanged)));
        public static readonly DependencyProperty RainFallProperty =
DependencyProperty.Register("RainFall", typeof(double), typeof(WeatherControl), new FrameworkPropertyMetadata(0.0,
new PropertyChangedCallback(OnValueChanged)));


        public WeatherControl()
        {
            InitializeComponent();
        }

        public double Temperature
        {
            get
            {
                return (double)GetValue(TemperatureProperty);
            }

            set
            {


                SetValue(TemperatureProperty, value);

            }

        }

        public double Humidity
        {
            get
            {
                return (double)GetValue(HumidityProperty);
            }

            set
            {


                SetValue(HumidityProperty, value);

            }

        }

        public double WindDirection
        {

            get
            {
               return (double)  GetValue(WindDirectionProperty);
            }

            set
            {

              
                SetValue(WindDirectionProperty, value);

            }
        }

       
        public double WindSpeed
        {

            get
            {
                return (double)GetValue(WindSpeedProperty);
            }

            set
            {


                SetValue(WindSpeedProperty, value);

            }
        }


        public double PM25
        {

            get
            {
                return (double)GetValue(PM25Property);
            }

            set
            {


                SetValue(PM25Property, value);

            }
        }


        public double WaterLevel
        {

            get
            {
                return (double)GetValue(WaterLevelProperty);
            }

            set
            {


                SetValue(WaterLevelProperty, value);

            }
        }


        public double RainFall
        {

            get
            {
                return (double)GetValue(RainFallProperty);
            }

            set
            {


                SetValue(RainFallProperty, value);

            }
        }


        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WeatherControl ctl = d as WeatherControl;
            if (e.Property.Name == "WindDirection")
            {
                Storyboard stb = ctl.FindResource("stbWndDir") as Storyboard;
                EasingDoubleKeyFrame keyFrame = ((ctl.Resources["stbWndDir"]
                   as Storyboard).Children[0]
                   as DoubleAnimationUsingKeyFrames).KeyFrames[0]
                   as EasingDoubleKeyFrame;
                keyFrame.Value = (double)e.NewValue - 90;
                stb.Stop();
                stb.Begin();
            }
            else if (e.Property.Name == "WindSpeed")
            {
                Storyboard stb = ctl.FindResource("stbWndSpd") as Storyboard;
                EasingDoubleKeyFrame keyFrameXSCale = ((ctl.Resources["stbWndSpd"]
                   as Storyboard).Children[0]
                   as DoubleAnimationUsingKeyFrames).KeyFrames[0]
                   as EasingDoubleKeyFrame;
                EasingDoubleKeyFrame keyFrameYSCale = ((ctl.Resources["stbWndSpd"]
                  as Storyboard).Children[1]
                  as DoubleAnimationUsingKeyFrames).KeyFrames[0]
                  as EasingDoubleKeyFrame;
                double wndseppd=0;

               keyFrameYSCale.Value=   keyFrameXSCale.Value = (double)e.NewValue / 11;
                stb.Stop();
                stb.Begin();
            }
            else if (e.Property.Name == "Temperature")
            {
                ctl.rTemperature.Text = string.Format("{0:0.0}",e.NewValue);
            }
            else if (e.Property.Name == "Humidity")
            {
                ctl.rHumidity.Text = string.Format("{0:0.0}", (double)e.NewValue*100); 
            }
            else if (e.Property.Name == "RainFall")
            {
               
                
                ctl.rRainfall.Text = string.Format("{0:0.0}", (double)e.NewValue  ); 
            }
            else if (e.Property.Name == "PM25")
            {
                Storyboard stb = ctl.FindResource("stbPM25") as Storyboard;
                EasingDoubleKeyFrame keyFramePM25 = ((ctl.Resources["stbPM25"]
                   as Storyboard).Children[0]
                   as DoubleAnimationUsingKeyFrames).KeyFrames[0]
                   as EasingDoubleKeyFrame;

                double pmvalue = (double)e.NewValue;
                if (pmvalue > 350)
                    pmvalue = 350;
                keyFramePM25.Value = pmvalue / 350;
                stb.Stop();
                stb.Begin();
            }
            else if (e.Property.Name == "WaterLevel")
            {
                Storyboard stb = ctl.FindResource("stbWaterLevel") as Storyboard;
                EasingDoubleKeyFrame keyFrameWaterLevel = ((ctl.Resources["stbWaterLevel"]
                   as Storyboard).Children[0]
                   as DoubleAnimationUsingKeyFrames).KeyFrames[0]
                   as EasingDoubleKeyFrame;

                double waterlevel = (double)e.NewValue;
                if (waterlevel > 50)
                    waterlevel = 50;
                keyFrameWaterLevel.Value = waterlevel / 50;
                stb.Stop();
                stb.Begin();

            }
            


        }

    }
}
