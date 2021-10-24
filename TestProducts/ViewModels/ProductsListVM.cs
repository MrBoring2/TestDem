using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProducts.Helpers;
using TestProducts.Models;
using TestProducts.Services;

namespace TestProducts.ViewModels
{
    public class ProductsListVM : BaseVM
    {
        private DataBaseService dataBaseService;
        private ObservableCollection<Products> products;
        private int itemsOnPage = 3;
        public ObservableCollection<Products> Products { get => products; set { products = value; } }
        public ObservableCollection<Products> FilterProducts 
        { 
            get => new ObservableCollection<Products>(Products.Skip(CurrentPage * itemsOnPage).Take(itemsOnPage)); 
        }

        public ProductsListVM()
        {
            dataBaseService = new DataBaseService();
            LoadProducts();
        }

        private RelayCommand editProduct;
        private RelayCommand addProduct;
        private RelayCommand removeProduct;
        
        public RelayCommand EditProduct
        {
            get
            {
                return editProduct ?? (editProduct = new RelayCommand(obj =>
                {
                    
                }));
            }
        }
        public RelayCommand AddProduct
        {
            get
            {
                return addProduct ?? (addProduct = new RelayCommand(obj =>
                {
                    WindowNavigation.Instanse.OpenModalWindow(new ProductVM());
                }));
            }
        }
        public RelayCommand RemoveProduct
        {
            get
            {
                return removeProduct ?? (removeProduct = new RelayCommand(obj =>
                {

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
                    if(CurrentPage + 1 < MaxPage)
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
                    if(CurrentPage > 0)
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
                OnPropertyChanged("FilterProducts");
            }
        }

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
