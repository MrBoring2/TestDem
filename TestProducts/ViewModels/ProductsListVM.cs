using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TestProducts.Helpers;
using TestProducts.Models;
using TestProducts.Services;

namespace TestProducts.ViewModels
{
    public class ProductsListVM : BaseVM
    {
        private Dispatcher Dispatcher { get; set; }
        private DataBaseService dataBaseService;
        private ObservableCollection<Products> products;
        private int itemsOnPage = 3;

        private Products selectedProduct;
        public ObservableCollection<Products> Products { get => products; set { products = value; } }
        public ObservableCollection<Products> FilterProducts
        {
            get => new ObservableCollection<Products>(Products.Skip(CurrentPage * itemsOnPage).Take(itemsOnPage));
        }

        public ProductsListVM()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
            AddProduct = new RelayCommand(AddProductAsync);
            EditProduct = new RelayCommand(EditProductAsync);
            dataBaseService = new DataBaseService();
            LoadProducts();
        }

        private RelayCommand editProduct;
        private RelayCommand removeProduct;

        public RelayCommand EditProduct { get; set; }

        async void EditProductAsync(object _)
        {
            var productsVM = new ProductEditVM(_ as Products);
            await Task.Run(() => WindowNavigation.Instanse.OpenModalWindow(productsVM));
            if (productsVM.DialogResult == true)
            {
                
            }

        }

        public RelayCommand AddProduct { get; set; }

        ///
        async void AddProductAsync(object _)
        {
            var productsVM = new ProductVM();
            await Task.Run(() => WindowNavigation.Instanse.OpenModalWindow(productsVM));
            if (productsVM.DialogResult == true)
            {
                dataBaseService.AddProduct(productsVM.NewProduct);
                LoadProducts();
                OnPropertyChanged("FilterProducts");
            }

        }


        private RelayCommand test;
        public RelayCommand Test
        {
            get
            {
                return test ?? (test = new RelayCommand(obj =>
                {
                    MessageBox.Show("dsad");
                }));
            }
        }
        public RelayCommand RemoveProduct
        {
            get
            {
                return removeProduct ?? (removeProduct = new RelayCommand(obj =>
                {
                    dataBaseService.RemoveProduct((obj as Products).ProductName);
                    LoadProducts();
                    OnPropertyChanged("FilterProducts");
                }));
            }
        }

        private RelayCommand nextPage;
        private RelayCommand previousPage;
        public RelayCommand NextPage
        {
            get
            {
                return nextPage ?? (nextPage = new RelayCommand(obj =>
                {
                    if (CurrentPage + 1 < MaxPage)
                        CurrentPage++;
                }));
            }
        }
        public RelayCommand PreviousPage
        {
            get
            {
                return previousPage ?? (previousPage = new RelayCommand(obj =>
                {
                    if (CurrentPage > 0)
                        CurrentPage--;
                }));
            }
        }

        private void UpdatePage()
        {
            //FilterProducts = new ObservableCollection<Products>(Products.Skip(CurrentPage * itemsOnPage).Take(itemsOnPage));
        }
        private int MaxPage
        {
            get => Convert.ToInt32(Math.Ceiling((float)Products.Count / (float)itemsOnPage));
        }
        private int currentPage;
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                OnPropertyChanged();
                OnPropertyChanged("FilterProducts");
            }
        }

        public Products SelectedProduct { get => selectedProduct; set { selectedProduct = value; OnPropertyChanged(); } }

        private void LoadProducts()
        {
            //Products = new ObservableCollection<Products>(new List<Products>
            //{
            //    new Products{ Amount = 5, Type= "dsad", MaterialToProduct=null, ProductName="dsad", Supplier ="dsada", ImagePath="Images/pod2.jpg" }
            //});
            Products = new ObservableCollection<Products>(dataBaseService.GetProducts());
        }
    }
}
