using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities
{
   public class Category:BaseEntity
    {
        public List<Product>? Products { get; set; }
        public List<CategoryLanguage> CategoryLanguages { get; set; }
    }
}
