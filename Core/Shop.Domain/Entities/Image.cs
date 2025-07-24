using Shop.Domain.Entities.Common;
using Shop.Domain.Entities.WebUIEntites;

namespace Shop.Domain.Entities
{
    public class Image:BaseEntity
    {
        public string Path { get; set; }
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
        public Guid? HomeSliderItemId { get; set; }
        public HomeSliderItem?  HomeSliderItem { get; set; }
        public Guid? TopCategoryAreaId { get; set; }
        public TopCategoryArea? TopCategoryArea { get; set; }


    }
}
