using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProducts.Helpers;

namespace TestProducts.ViewModels
{
    public class BaseProductVM : BaseVM
    {
        protected RelayCommand saveProduct;
        public virtual RelayCommand SaveProduct { get; }
    }
}
