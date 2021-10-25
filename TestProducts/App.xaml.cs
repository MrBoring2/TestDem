using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TestProducts.Helpers;
using TestProducts.ViewModels;
using TestProducts.Views;

namespace TestProducts
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            WindowNavigation.Instanse.RegisterWindowType<ProductsListVM, ProductsList>();
            WindowNavigation.Instanse.RegisterWindowType<ProductVM, Product>();
            WindowNavigation.Instanse.RegisterWindowType<ProductEditVM, Product>();
            WindowNavigation.Instanse.RegisterWindowType<SelectMaterialVM, SelectMaterial>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            WindowNavigation.Instanse.ShowWindow(new ProductsListVM());
        }
    }
}
