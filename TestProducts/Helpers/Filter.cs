using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProducts.Models;

namespace TestProducts.Helpers
{
    public class Filter
    {
        public string Title { get; set; }
        public string Property { get; set; }

        public Filter(string title, string property)
        {
            Title = title;
            Property = property;
        }

        public bool IsContains(object item, string parameter)
        {
            if(item is Products p)
            {
                var value = p[parameter];
                return value.ToString().Contains(parameter);
            }
            return false;
        }

        public bool IsEquals(object item, string parameter)
        {
            if(item is Products p)
            {
                var value = p[Property];
                return value.ToString().Equals(parameter);
            }
            return false;
        }
    }
}
