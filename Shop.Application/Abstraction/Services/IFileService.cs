using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services
{
    public interface IFileService
    {
        public Task<IDataResult<string>> SaveImageAsync(Microsoft.AspNetCore.Http.IFormFile image, bool isProductImage = false);
         public Task<IDataResult<List<string>>> SaveImageRangeAsync(Microsoft.AspNetCore.Http.IFormFileCollection images, bool isProductImages = false);
        public IResult RemoveFileRange(List<string> FilePaths);
        public IResult RemoveFile(string FilePaths);
        public IDataResult<List<string>> SaveOrderPdf(List<OrderProductDTO> items, OrderShippingMethodDTO shippingMethod, OrderPaymentMethodDTO paymentMethod, OrderUserInfoDTO userInfoDTO);
    }


}
