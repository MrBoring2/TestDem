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
                return db.Products.ToList();
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
    }
}
