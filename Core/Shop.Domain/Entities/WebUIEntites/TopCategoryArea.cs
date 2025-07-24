using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities.WebUIEntites
{
   public class TopCategoryArea:BaseEntity
    {
        public bool IsActive { get; set; }
        public Guid ImageId { get; set; }
        public Image Image { get; set; }
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<TopCategoryAreaLanguage> TopCategoryAreaLanguages { get; set; }
    }
}
