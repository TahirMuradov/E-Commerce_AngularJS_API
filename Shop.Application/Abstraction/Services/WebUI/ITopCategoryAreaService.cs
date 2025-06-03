using Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services.WebUI
{
 public   interface ITopCategoryAreaService
    {
        public Task<IResult> AddTopCategoryAreaAsync(AddTopCategoryAreaDTO addTopCategoryAreaDTO, string LangCode);
        public Task<IResult> UpdateTopCategoryAreaAsync(UpdateTopCategoryAreaDTO updateTopCategoryAreaDTO, string LangCode);
        public IResult ChangeVisibleTopCategoryArea(Guid Id, string LangCode);
        public IResult RemoveTopCategoryArea(Guid Id, string LangCode);
        public Task<IDataResult<PaginatedList<GetTopCategoryAreaDTO>>> GetTopCategoryAreaByPageOrSearchAsync(string LangCode, int page, string? search = null);
        public IDataResult<IQueryable<GetTopCategoryAreaForUIDTO>> GetTopCategoryAreaForUI(string LangCode);
        public Task<IDataResult<GetTopCategoryAreaForUpdateDTO>> GetTopcategoryAreaForUpdateAsync(Guid Id, string LangCode);


    }
}
