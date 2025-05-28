using Shop.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Entities
{
   public class Size:BaseEntity
    {
        public string Content { get; set; }

        public List<SizeProduct> SizeProducts { get; set; }
        public List<SoldProduct>? SoldProducts { get; set; }
    }
}
