using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestProducts.Services
{
    public class DialogCloser
    {
        public static readonly DependencyProperty DialogReultProperty =
            DependencyProperty.RegisterAttached
            ("DialogResult", typeof(bool), typeof(DialogCloser), new PropertyMetadata(DialogResultChanged));
        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = d as Window;
            if (window != null)
                window.DialogResult = e.NewValue as bool?;
        }
    }
}
