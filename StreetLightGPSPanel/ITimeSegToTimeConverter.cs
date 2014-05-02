using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StreetLightPanel
{
    public class ITimeSegToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int time = System.Convert.ToInt32(value);
            return string.Format("{0:00}:{1:00}", time / 60, time % 60);


        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Text.RegularExpressions.Regex regx = new System.Text.RegularExpressions.Regex(@"\d{1,2}:\d{1,2}");
            if (!regx.Match(value.ToString()).Success)
                // return value;
                return -1;
            string[] data = value.ToString().Split(new char[] { ':' });
            int hour = System.Convert.ToInt32(data[0]);
            int min = System.Convert.ToInt32(data[1]);
            if (hour >= 24)
                return -1;
            if (min >= 60)
                return -1;
            return hour * 60 + min;

        }
    }
}
