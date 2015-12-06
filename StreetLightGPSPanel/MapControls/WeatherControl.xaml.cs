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
FrameworkPropertyMetadataOptions.Inherits));

        public WeatherControl()
        {
            InitializeComponent();
        }

        public double WindDirection
        {

            get
            {
               return (double)  GetValue(WindDirectionProperty);
            }

            set
            {

               Storyboard   stb= this.FindResource("stbWindDir") as Storyboard;
               EasingDoubleKeyFrame keyFrame = ((this.Resources["stbWindDir"]
                  as Storyboard).Children[0]
                  as DoubleAnimationUsingKeyFrames).KeyFrames[0]
                  as EasingDoubleKeyFrame;
               keyFrame.Value = value - 90;
                SetValue(WindDirectionProperty, value);

            }
        }

    }
}
