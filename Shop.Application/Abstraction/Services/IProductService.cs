using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services
{
    public interface IProductService
    {
        public Task<IResult> AddProductAsync(AddProductDTO addProductDTO, string LangCode);
        public Task<IResult> UpdateProductAsync(UpdateProductDTO updateProductDTO, string LangCode);
        public IResult DeleteProduct(Guid id,string LangCode);
        public IDataResult<GetProductDetailDTO> GetProductById(Guid id, string LangCode);
        public Task<IDataResult<PaginatedList<GetProductDTO>>> GetAllProductByPageOrSearchAsync(int page,string LangCode, string? search = null);

        public IDataResult<IQueryable<GetProductDTO>> GetProductByFeatured(string LangCode);
    }
}
