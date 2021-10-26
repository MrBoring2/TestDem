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

        private string searchText;
        public string SearchText { get => searchText; set { searchText = value; OnPropertyChanged(); OnPropertyChanged(nameof(FilterProducts)); } }

        private bool orderByAscend;
        public bool OrderByAscend { get => orderByAscend; set { orderByAscend = value; OnPropertyChanged(); OnPropertyChanged(nameof(FilterProducts)); } }

        private List<Filter> filterTypes;
        public List<Filter> FilterTypes { get => filterTypes; set { filterTypes = value; OnPropertyChanged(); } }

        private Filter selectedType;
        public Filter SelectedType { get => selectedType; set { selectedType = value; OnPropertyChanged(); OnPropertyChanged(nameof(FilterProducts)); } }

        private Products selectedProduct;
        public ObservableCollection<Products> Products { get => products; set { products = value; } }

        private List<Filter> sortParameters;
        public List<Filter> SortParameters { get => sortParameters; set { sortParameters = value; OnPropertyChanged(); } }

        private Filter sortBy;
        public Filter SortBy { get => sortBy; set { sortBy = value; OnPropertyChanged(); OnPropertyChanged(nameof(FilterProducts)); } }
        public ObservableCollection<Products> FilterProducts
        {
            get
            {
                OnPropertyChanged(nameof(DisplayPages));
                if (OrderByAscend)
                {
                    return new ObservableCollection<Products>
                    (Products
                    .Where(p => p.ProductName
                    .Contains(SearchText))
                    .Where(p => SelectedType.Title.Equals("Все типы") ? p.Type.Contains("") : p.Type.Equals(SelectedType.Title))
                    .OrderBy(p => p.GetModelPropert(SortBy.Property)).Skip(CurrentPage * itemsOnPage).Take(itemsOnPage)); ;
                }
                else return new ObservableCollection<Products>
                    (Products.Where(p => p.ProductName
                    .Contains(SearchText))
                    .Where(p => SelectedType.Title.Equals("Все типы") ? p.Type.Contains("") : p.Type.Equals(SelectedType.Title))
                    .OrderByDescending(p => p.GetModelPropert(SortBy.Property)).Skip(CurrentPage * itemsOnPage).Take(itemsOnPage));
            }
        }


        public string DisplayPages
        {
            get => $"{CurrentPage + 1}/{MaxPage}";
        }

        public ProductsListVM()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
            AddProduct = new RelayCommand(AddProductAsync);
            EditProduct = new RelayCommand(EditProductAsync);
            dataBaseService = new DataBaseService();
            LoadProducts();
            SortParameters = new List<Filter>
            {
                new Filter("Название", "ProductName"),
                new Filter("Количество", "Amount"),
                new Filter("Поставщик", "Supplier"),
                new Filter("Тип", "Type")
            };
            FilterTypes = new List<Filter>
            {
                new Filter("Все типы", null),
                new Filter("Подушка", "Type"),
                new Filter("Стол", "Type")
            };
            SearchText = string.Empty;
            SortBy = SortParameters[0];
            SelectedType = FilterTypes[0];
            OrderByAscend = true;
        }

        private RelayCommand removeProduct;

        public RelayCommand EditProduct { get; set; }

        async void EditProductAsync(object _)
        {
            var productsVM = new ProductEditVM(_ as Products);
            await Task.Run(() => WindowNavigation.Instanse.OpenModalWindow(productsVM));
            if (productsVM.DialogResult == true)
            {
                dataBaseService.UpdateProduct(productsVM.CurrentProduct);
                LoadProducts();
                OnPropertyChanged("FilterProducts");
            }

        }

        public RelayCommand AddProduct { get; set; }


        async void AddProductAsync(object _)
        {
            var productsVM = new ProductVM();
            await Task.Run(() => WindowNavigation.Instanse.OpenModalWindow(productsVM));
            if (productsVM.DialogResult == true)
            {
                dataBaseService.AddProduct(productsVM.CurrentProduct);
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

            get
            {

                return Convert.ToInt32(Math.Ceiling((float)Products
                    .Where(p => p.ProductName
                    .Contains(SearchText))
                    .Where(p => SelectedType.Title.Equals("Все типы") ? p.Type.Contains("") : p.Type.Equals(SelectedType.Title)).Count() / (float)itemsOnPage));

            }
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

                OnPropertyChanged("DisplayPages");
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
