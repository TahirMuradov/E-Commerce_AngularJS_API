using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs
{
   public class PageDTO
    {
        public readonly int MaxProductCount = 9;
        public int PageSize { get; set; }
        public int ActivePage { get; set; } = 1;
    }
}
