using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TestProducts.Helpers.Converters
{
    public class RadioButtonCOnverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(System.Convert.ToBoolean(System.Convert.ToInt32(parameter)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? System.Convert.ToBoolean(System.Convert.ToInt32(parameter)) : Binding.DoNothing;
        }
    }
}
