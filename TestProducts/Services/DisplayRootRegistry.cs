using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestProducts.ViewModels;

namespace TestProducts.Services
{
    public class DisplayRootRegistry
    {
        Dictionary<Type, Type> WindowsWithViewModels = new Dictionary<Type, Type>();

        public void RegisterVM<VM, Win>()
            where VM : BaseVM
            where Win : Window
        {
            var vmType = typeof(VM);
            var windowType = typeof(Win);

            WindowsWithViewModels[vmType] = windowType;
        }

        public Window CreateWindowWihhVM(object vm)
        {
            Type windowType = null;

            Type vmType = vm.GetType();

            while (vmType != null && !WindowsWithViewModels.TryGetValue(vm.GetType(), out windowType))
                vmType = vmType.BaseType;

            if (windowType == null)
                throw new Exception("Нет зарегестрированной формы!");

            var window = (Window)Activator.CreateInstance(windowType);
            window.DataContext = vm;
            return window;
        }

        Dictionary<BaseVM, Window> OpenWindows = new Dictionary<BaseVM, Window>();

        public void ShowWindow(BaseVM viewModel)
        {
            var window = CreateWindowWihhVM(viewModel);
            window.Show();
            OpenWindows.Add(viewModel, window);
        }

        public void HideWindow(BaseVM viewModel)
        {
            var window = OpenWindows[viewModel];
            if (window != null)
            {
                window.Close();
                OpenWindows.Remove(viewModel);
            }
            else throw new Exception("Такой формы нет");
        }

        public async Task ShowMoldaWindow(BaseVM viewModel)
        {
            var window = CreateWindowWihhVM(viewModel);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            await window.Dispatcher.InvokeAsync(() => window.ShowDialog());
        }
    }
}
