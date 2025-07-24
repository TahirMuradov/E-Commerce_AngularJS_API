using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services
{
    public interface IFileService
    {
        public Task<IDataResult<string>> SaveImageAsync(string LangCode,Microsoft.AspNetCore.Http.IFormFile image, bool isProductImage = false);
         public Task<IDataResult<List<string>>> SaveImageRangeAsync(string LangCode,Microsoft.AspNetCore.Http.IFormFileCollection images, bool isProductImages = false);
        public IResult RemoveFileRange(string LangCode,List<string> FilePaths);
        public IResult RemoveFile(string LangCode,string FilePaths);
        public IDataResult<List<string>> SaveOrderPdf(string LangCode,List<OrderProductDTO> items, OrderShippingMethodDTO shippingMethod, OrderPaymentMethodDTO paymentMethod, OrderUserInfoDTO userInfoDTO);
    }


}
