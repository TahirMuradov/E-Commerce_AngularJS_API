using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Domain.Enums;

namespace Shop.Application.Abstraction.Services
{
   public interface IOrderService
    {
        public Task<IResult> AddOrderAsync(AddOrderDTO addOrderDTO,string LangCode);
        public Task<IDataResult<GetOrderDetailDTO>> GetOrderByIdAsync(Guid orderId, string LangCode);
        public Task<IDataResult<PaginatedList<GetOrderByUserDTO>>> GetAllOrdersByUserIdAsync(string userId, int page, string LangCode);
        public Task<IResult> UpdateOrderStatusAsync(Guid orderId, OrderStatus status, string LangCode);
        public Task<IResult> UpdateOrderAsync(UpdateOrderDTO updateOrderDTO, string LangCode);
        public Task<IDataResult<PaginatedList<GetOrderDTO>>> GetAllOrdersByPageAsync(int page, string LangCode);

        public Task<IResult> DeleteOrderAsync(Guid orderId, string LangCode);



    }
}
