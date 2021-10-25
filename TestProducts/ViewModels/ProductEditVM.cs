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
    public class ProductEditVM : BaseVM
    {
        private DataBaseService dataBaseService;
        private string name;
        private string type;
        private string amount;
        private string supplier;
        private string filePath;
        private string tempFileName;
        private byte[] image;
        private Products EditingProduct { get; set; }
        public ObservableCollection<Materials> Materials { get; private set; }
        private bool? dialogResult;
        public ProductEditVM(Products product)
        {
            dataBaseService = new DataBaseService();
            Materials = new ObservableCollection<Materials>();
            EditingProduct = product;
            Name = EditingProduct.ProductName;
            Type = EditingProduct.Type;
            Amount = EditingProduct.Amount.ToString();
            Supplier = EditingProduct.Supplier;
            FilePath = EditingProduct.ImagePath;
            Image = File.ReadAllBytes("../../Resourses/" + FilePath);
            foreach (var item in EditingProduct.MaterialToProduct)
            {
                Materials.Add(item.Materials);
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

        private RelayCommand add;
        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new RelayCommand(obj =>
                {
                    try
                    {
                        File.Copy(TempFileName, @"../../Resourses/Images/" + FilePath);
                        Products product = new Products();
                        product.Amount = Convert.ToInt32(Amount);
                        product.ImagePath = @"Images/" + FilePath;
                        product.ProductName = Name;
                        product.Supplier = Supplier;
                        product.Type = Type;
                        dataBaseService.AddProduct(product);
                        DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        DialogResult = false;
                    }
                }));
            }
        }
      
        

        public byte[] Image { get => image; set { image = value; OnPropertyChanged(); } }

        public string Supplier { get => supplier; set { supplier = value; OnPropertyChanged(); } }

        public string TempFileName { get => tempFileName; set => tempFileName = value; }
        public bool? DialogResult { get => dialogResult; set { dialogResult = value; OnPropertyChanged(); } }
    }
}
