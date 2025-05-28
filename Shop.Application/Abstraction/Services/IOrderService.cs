using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services
{
   public interface IOrderService
    {
        public Task<IResult> AddOrderAsync(AddOrderDTO addOrderDTO,string LangCode);
      

    }
}
