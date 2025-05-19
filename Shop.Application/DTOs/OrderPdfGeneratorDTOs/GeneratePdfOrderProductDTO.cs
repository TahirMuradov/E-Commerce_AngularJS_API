using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.DTOs.OrderPdfGeneratorDTOs
{
    public class GeneratePdfOrderProductDTO
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
