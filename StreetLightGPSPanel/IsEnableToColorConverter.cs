using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace StreetLightPanel
{
    public class IsEnableToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return new SolidColorBrush(Colors.Black);
            else
                return new SolidColorBrush(Colors.Red);
            // throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


   public class IsEnableToStreetColorConverter : IValueConverter
   {
       public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
       {
           int status = (int)value;
           if (status == -1)
               return Colors.Gray;

           if (status == 1)
               return Colors.Yellow;
           else
               return Colors.Red;
           // throw new NotImplementedException();
       }

       public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
       {
           throw new NotImplementedException();
       }
   }
}
