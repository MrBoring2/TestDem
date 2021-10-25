using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestProducts.ViewModels;

namespace TestProducts.Helpers
{
    public class OpenDialogCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public async void Execute(object parameter)
        {
            await App.Current.Dispatcher.InvokeAsync(() => WindowNavigation.Instanse.OpenModalWindow(new ProductVM()));
        }
    }
}
