using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities.WebUIEntites
{
   public class TopCategoryArea:BaseEntity
    {
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<TopCategoryAreaLanguage> TopCategoryAreaLanguages { get; set; }
    }
}
