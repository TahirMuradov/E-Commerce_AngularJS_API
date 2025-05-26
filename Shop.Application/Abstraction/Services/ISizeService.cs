using Shop.Application.DTOs.SizeDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services
{
   public interface ISizeService
    {

        public IResult AddSize(AddSizeDTO addSizeDTO, string locale);
        public IResult UpdateSize(UpdateSizeDTO updateSizeDTO, string locale);
        public IResult DeleteSize(Guid id, string locale);
        public IDataResult<GetSizeDTO> GetSizeById(Guid id, string locale);
        public IDataResult<IQueryable<GetSizeDTO>> GetAllSizes(string locale);
        public Task<IDataResult<PaginatedList<GetSizeDTO>>> GetAllSizesByPageAsync(int page, string locale);

    }
}
