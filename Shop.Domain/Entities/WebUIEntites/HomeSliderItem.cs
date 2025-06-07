using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities.WebUIEntites
{
  public  class HomeSliderItem : BaseEntity
    {
        public Guid ImageId { get; set; }
        public Image Image { get; set; }
        public bool IsActive { get; set; }
        public List<HomeSliderLanguage> Languages { get; set; }

    }
}
