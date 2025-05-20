using Shop.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Entities
{
   public class Category:BaseEntity
    {
        public List<Product> Products { get; set; }
        public List<CategoryLanguage> CategoryLanguages { get; set; }
    }
}
