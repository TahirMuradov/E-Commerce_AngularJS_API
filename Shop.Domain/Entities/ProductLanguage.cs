using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities
{
    public class ProductLanguage : BaseEntity
    {
        public string LanguageCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
