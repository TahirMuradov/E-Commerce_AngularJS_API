using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities.WebUIEntites
{
   public class TopCategoryAreaLanguage:BaseEntity
    {
        public string LangCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid TopCategoryAreaId { get; set; }
        public TopCategoryArea TopCategoryArea { get; set; }

    }
}
