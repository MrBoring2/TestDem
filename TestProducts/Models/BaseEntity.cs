using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProducts.Models
{
    public class BaseEntity
    {
        [NotMapped]
        public object this[string property]
        {
            get
            {
                if(property != string.Empty)
                {
                    var info = this.GetType().GetProperty(property);
                    return info.GetValue(this, null);
                }
                return null;
            }
        }

        public object GetModelPropert(string property)
        {
            return GetType().GetProperty(property).GetValue(this, null);
        }
    }
}
