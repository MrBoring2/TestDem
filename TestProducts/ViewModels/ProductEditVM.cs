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
    public class ProductEditVM : ProductVM
    {
        public ProductEditVM(Products product)
        {
            CurrentProduct = product;
            Name = CurrentProduct.ProductName;
            Type = CurrentProduct.Type;
            Amount = CurrentProduct.Amount.ToString();
            Supplier = CurrentProduct.Supplier;
            FilePath = CurrentProduct.ImagePath;
            Image = File.ReadAllBytes("../../Resourses/" + FilePath);
            TempFileName = FilePath;
        }
        public override RelayCommand SaveProduct
        {
            get
            {
                return saveProduct ?? (saveProduct = new RelayCommand(obj =>
                {
                    try
                    {
                        if(TempFileName != FilePath)
                            File.Copy(TempFileName, @"../../Resourses/Images/" + FilePath);
                        List<MaterialToProduct> list = new List<MaterialToProduct>();

                        CurrentProduct.Amount = Convert.ToInt32(Amount);
                        if(CurrentProduct.ImagePath != FilePath)
                            CurrentProduct.ImagePath = @"Images/" + FilePath;
                        CurrentProduct.ProductName = Name;
                        CurrentProduct.Supplier = Supplier;
                        CurrentProduct.Type = Type;
                        foreach (var item in Materials)
                        {
                            if (CurrentProduct.MaterialToProduct.Where(p => p.MaterialName.Equals(item.MaterialName)).FirstOrDefault() == null)
                            {
                                list.Add(new MaterialToProduct { ProductName = CurrentProduct.ProductName, MaterialName = item.MaterialName, AmountOfMaterial = materialsObjects[item] });
                            }                           
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
    }
}
