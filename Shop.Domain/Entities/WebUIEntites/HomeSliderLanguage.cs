using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities.WebUIEntites
{
   public class HomeSliderLanguage:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string LangCode { get; set; }
        public Guid HomeSliderItemId { get; set; }
        public HomeSliderItem HomeSliderItem { get; set; }
    }
}
