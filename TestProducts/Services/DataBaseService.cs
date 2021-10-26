using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProducts.Models;

namespace TestProducts.Services
{
    public class DataBaseService
    {
        public List<Products> GetProducts()
        {
            using(var db = new TestModel())
            {
                return db.Products.Include("MaterialToProduct").Include("MaterialToProduct.Materials").ToList();
            }
        }

        public List<Materials> GetMaterials()
        {
            using (var db = new TestModel())
            {
                return db.Materials.ToList();
            }
        }

        public void AddProduct(Products product)
        {
            using(var db = new TestModel())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
        }
        public void RemoveProduct(string productName)
        {
            using (var db = new TestModel())
            {
                var product = db.Products.Find(productName);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
            }
        }

        public void UpdateProduct(Products currentProduct)
        {
            using(var db = new TestModel())
            {
                if(currentProduct != null)
                {
                    db.Entry(currentProduct).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
    }
}
