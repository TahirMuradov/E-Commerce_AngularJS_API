using Shop.Application.DTOs.WebUI.NewArriwalDTOs;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services.WebUI
{
   public interface INewArriwalAreaService
    {
        public IDataResult<IQueryable<GetIsFeaturedCategoryDTO>> GetNewArriwalCategories(string LangCode);
        public IDataResult<IQueryable<GetNewArriwalProductDTO>> GetNewArriwalProducts(string LangCode);
    }
}
