using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities
{
   public class CategoryLanguage: BaseEntity
    {
        public string LanguageCode { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
   
    }
  
}
