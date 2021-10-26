using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestProducts.Services;
using TestProducts.ViewModels;

namespace TestProducts.Helpers
{
    public class WindowNavigation
    {
        private static WindowNavigation instanse;
        private DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
        private static object syncObject = new object();

        private WindowNavigation() { }

        public static WindowNavigation Instanse
        {
            get
            {
                if(instanse == null)
                    lock (syncObject)
                    {
                        if (instanse == null)
                            instanse = new WindowNavigation();
                    }
                return instanse;
            }
        }

        public void RegisterWindowType<VM, Win>()
            where VM : BaseVM
            where Win : Window
        {
            displayRootRegistry.RegisterVM<VM, Win>();
        }

        public void ShowWindow(BaseVM viewModel)
        {

            displayRootRegistry.ShowWindow(viewModel);
        }
        public void OpenAndHideWindow(BaseVM currentWindowVM, BaseVM newWindowVM)
        {
            if (currentWindowVM != null && newWindowVM != null)
            {
                displayRootRegistry.ShowWindow(newWindowVM);
                displayRootRegistry.HideWindow(currentWindowVM);
            }
        }

        public void OpenModalWindow(BaseVM windowVM)
        {
            if (windowVM != null)
            {
                displayRootRegistry.ShowMoldaWindow(windowVM);
            }
        }


    }
}
