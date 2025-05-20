using Microsoft.AspNetCore.Http;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;

namespace Shop.Application.Abstraction.Services
{
    public interface IFileService
    {
        public Task<string> SaveFileAsync(IFormFile file, string WebRootPath, bool isProductPicture = false);
        public Task<List<string>> SaveFileRangeAsync(List<IFormFile> file, string WebRootPath);
        public bool RemoveFileRange(List<string> FilePaths);
        public bool RemoveFile(string FilePaths);
        public List<string> SaveOrderPdf(List<GeneratePdfOrderProductDTO> items, ShippingMethodInOrderPdfDTO shippingMethod, PaymentMethodInOrderPdfDTO paymentMethod);
    }


}
