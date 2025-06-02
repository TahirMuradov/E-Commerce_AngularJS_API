using Shop.Application.DTOs.WebUI.HomeSliderItemDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services.WebUI
{
  public  interface IHomeSliderService
    {
        public Task<IResult> AddHomeSliderItemAsync(AddHomeSliderItemDTO addHomeSliderItemDTO,string LangCode);
        public Task<IResult> UpdateHomeSliderItemAsync(UpdateHomeSliderItemDTO updateHomeSliderItemDTO,string LangCode);
        public IResult DeleteHomeSliderItem(Guid Id, string LangCode);
        public Task<IDataResult<PaginatedList<GetHomeSliderItemDTO>>> GetAllHomeSliderAsync(string LangCode, int page);
        public Task<IDataResult<GetHomeSliderItemForUpdateDTO>> GetHomeSliderItemForUpdateAsync(Guid Id, string LangCode);
        public IDataResult<IQueryable<GetHomeSliderItemForUIDTO>> GetHomeSliderItemForUI(string LangCode);
    }
}
