using Shop.Application.DTOs.WebUI.DisCountAreaDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services.WebUI
{
    public interface IDiscountAreaService
    {
        public IResult AddDiscountArea(AddDisCountAreaDTO addDisCountAreaDTO, string langCode);
        public IResult UpdateDisCountArea(UpdateDisCountAreaDTO updateDisCountAreaDTO, string LangCode);
        public Task<IDataResult<GetDisCountAreaForUpdateDTO>> GetDisCountAreaForUpdateAsync(Guid Id, string LangCode);
        public IDataResult<IQueryable<GetDisCountAreaDTO>> GetAllDisCountArea(string LangCode);
        public Task<IDataResult<PaginatedList<GetDisCountAreaDTO>>> GetAllDisCountByPageOrSearchAsync(string LangCode, int page,string? search=null);
        public IResult Delete(Guid Id, string LangCode);
    }
}
