using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace slPanel
{
    public class IDailyPlanDateTimeConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
           // throw new NotImplementedException();
            DateTime dt =(DateTime) value ;
            return string.Format( "{0:00}:{1:00}",dt.Hour,dt.Minute);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
           
                DateTime dt;

                if (DateTime.TryParse(value.ToString(), out dt))
                    return new DateTime(1900, 1, 1, dt.Hour, dt.Minute, dt.Second);
                else

                    return null;
          
            }
           
           
          
        
    }
}
