using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities
{
   public class Size:BaseEntity
    {
        public string Content { get; set; }

        public List<SizeProduct> SizeProducts { get; set; }
        public List<SoldProduct>? SoldProducts { get; set; }
    }
}
