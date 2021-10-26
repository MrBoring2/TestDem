using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProducts.Helpers;
using TestProducts.Models;
using TestProducts.Services;

namespace TestProducts.ViewModels
{
    public class ProductVM : BaseProductVM
    {
        private DataBaseService dataBaseService;
        private string name;
        private string type;
        private string amount;
        private string supplier;
        private string filePath;
        private string tempFileName;
        private byte[] image;
        private ObservableCollection<Materials> materials;
        protected Dictionary<Materials, int> materialsObjects;
        public Products CurrentProduct { get; protected set; }
        private bool? dialogResult;
        public ProductVM()
        {
            CurrentProduct = new Products();
            dataBaseService = new DataBaseService();
            AddMaterials = new RelayCommand(AddMaterialsAsync);
            Materials = new ObservableCollection<Materials>();
            materialsObjects = new Dictionary<Materials, int>();
        }

        private async void AddMaterialsAsync(object obj)
        {
            var materialsVM = new SelectMaterialVM();
            await Task.Run(() => WindowNavigation.Instanse.OpenModalWindow(materialsVM));
            if (materialsVM.DialogResult == true)
            {
                var item = materialsVM.SelectedMaterial;

                if (Materials.Where(p => p.MaterialName.Equals(item.MaterialName)).FirstOrDefault() == null)
                {
                    Materials.Add(item);
                    materialsObjects.Add(item, Convert.ToInt32(materialsVM.Amount));
                }

            }
        }

        public string Amount { get => amount; set { amount = value; OnPropertyChanged(); } }
        public string FilePath { get => filePath; set { filePath = value; OnPropertyChanged(); } }
        public string Type { get => type; set { type = value; OnPropertyChanged(); } }
        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        private RelayCommand loadImage;
        public RelayCommand LoadImage
        {
            get
            {
                return loadImage ?? (loadImage = new RelayCommand(obj =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == true)
                    {
                        Image = File.ReadAllBytes(openFileDialog.FileName);
                        TempFileName = openFileDialog.FileName;
                        FilePath = Guid.NewGuid().ToString() + ".png";



                    }
                }));
            }
        }
        public override RelayCommand SaveProduct 
        {        
            get
            {
                return saveProduct ?? (saveProduct = new RelayCommand(obj =>
                {
                    try
                    {
                        File.Copy(TempFileName, @"../../Resourses/Images/" + FilePath);
                        List<MaterialToProduct> list = new List<MaterialToProduct>();

                        CurrentProduct.Amount = Convert.ToInt32(Amount);
                        CurrentProduct.ImagePath = @"Images/" + FilePath;
                        CurrentProduct.ProductName = Name;
                        CurrentProduct.Supplier = Supplier;
                        CurrentProduct.Type = Type;
                        foreach (var item in Materials)
                        {
                            list.Add(new MaterialToProduct { ProductName = CurrentProduct.ProductName, MaterialName = item.MaterialName, AmountOfMaterial = materialsObjects[item] });
                        }
                        CurrentProduct.MaterialToProduct = list;
                        DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        DialogResult = false;
                    }
                }));
            }
        }
        public RelayCommand AddMaterials { get; set; }

        public byte[] Image { get => image; set { image = value; OnPropertyChanged(); } }

        public string Supplier { get => supplier; set { supplier = value; OnPropertyChanged(); } }

        public string TempFileName { get => tempFileName; set => tempFileName = value; }
        public bool? DialogResult { get => dialogResult; set { dialogResult = value; OnPropertyChanged(); } }

        public ObservableCollection<Materials> Materials { get => materials; set { materials = value; OnPropertyChanged(); } }
    }
}
