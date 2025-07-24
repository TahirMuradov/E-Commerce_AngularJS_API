using Shop.Application.DTOs.WebUI.DisCountAreaDTOs;
using Shop.Application.DTOs.WebUI.HomeSliderItemDTOs;
using Shop.Application.DTOs.WebUI.NewArriwalDTOs;
using Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs;

namespace Shop.Application.DTOs.WebUI
{
   public class GetHomeAllDataDTO
    {
        public IQueryable<GetDisCountAreaForUiDTO> DisCountAreas { get; set; }
        public IQueryable<GetHomeSliderItemForUIDTO> HomeSliderItems { get; set; }
        public IQueryable<GetTopCategoryAreaForUIDTO> TopCategoryAreas { get; set; }
        public IQueryable<GetNewArriwalProductDTO> NewArriwalProducts { get; set; }
        public IQueryable<GetIsFeaturedCategoryDTO> IsFeaturedCategorys { get; set; }
    }
}
