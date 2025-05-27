using Microsoft.AspNetCore.Http;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services
{
    public interface IFileService
    {
        public Task<IDataResult< string>> SaveFileAsync(IFormFile file, bool isProductPicture = false);
        public Task<IDataResult< List<string>>> SaveFileRangeAsync(List<IFormFile> file);
        public IResult RemoveFileRange(List<string> FilePaths);
        public IResult RemoveFile(string FilePaths);
        public IDataResult< List<string>> SaveOrderPdf(List<GeneratePdfOrderProductDTO> items, ShippingMethodInOrderPdfDTO shippingMethod, PaymentMethodInOrderPdfDTO paymentMethod);
    }


}
