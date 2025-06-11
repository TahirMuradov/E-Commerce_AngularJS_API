using Shop.Application.DTOs.PaymentMethodDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services
{
   public interface IPaymentMethodService
    {
        public IResult AddPaymentMethod(AddPaymentMethodDTO addPaymentMethodDTO, string locale);
        public IResult UdpatePaymentMethod(UpdatePaymentMethodDTO updatePaymentMethodDTO, string locale);
        public IResult DeletePaymentMethod(Guid id, string locale);
        public Task<IDataResult<GetPaymentMethodDetailDTO>> GetPaymentMethodByIdAsync(Guid id, string locale);
        public IDataResult<IQueryable<GetPaymentMethodDTO>> GetAllPaymentMethods(string locale);
        public Task<IDataResult<PaginatedList<GetPaymentMethodDTO>>> GetAllPaymentMethodsByPageOrSearchAsync(int page, string locale, string? search = null);
    }
}
