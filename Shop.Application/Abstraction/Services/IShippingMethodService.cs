using Shop.Application.DTOs.ShippingMethodDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services
{
   public interface IShippingMethodService
    {
        public IResult AddShippingMethod(AddShippingMethodDTO addShippingMethodDTO, string locale);
        public IResult UdpateShippingMethod(UpdateShippingMethodDTO updateShippingMethodDTO, string locale);
        public IResult DeleteShippingMethod(Guid id, string locale);
        public IDataResult<GetShippingMethodDetailDTO> GetShippingMethodById(Guid id, string locale);
        public IDataResult<IQueryable<GetShippingMethodDTO>> GetAllShippingMethods(string locale);
        public Task<IDataResult<PaginatedList<GetShippingMethodDTO>>> GetAllShippingMethodsByPageAsync(int page, string locale);

    }
}
