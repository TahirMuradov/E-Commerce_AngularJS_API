using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities
{
   public class Product: BaseEntity
    {
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public decimal  DisCount { get; set; }
        public bool IsFeatured { get; set; }
        public List<Image> Images { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public List<SizeProduct>? SizeProducts { get; set; }
        public List<ProductLanguage> ProductLanguages { get; set; }
        public List<SoldProduct>? SoldProducts { get; set; }


    }
}
