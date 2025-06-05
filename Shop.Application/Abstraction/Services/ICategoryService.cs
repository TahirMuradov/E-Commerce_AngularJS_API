using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services
{
  public  interface ICategoryService
    {
        public IResult AddCategory(AddCategoryDTO addCategoryDTO ,string locale);
        public IResult UpdateCategory(UpdateCategoryDTO updateCategoryDTO ,string locale);
        public IResult DeleteCategory(Guid Id,string locale);
        public Task<IDataResult<PaginatedList<GetCategoryDTO>>> GetAllCategoryByPageOrSearchAsync(string locale, int page = 1, string? search=null);
        public IDataResult<IQueryable<GetCategoryDTO>> GetAllCategory(string locale);
        public IDataResult<GetCategoryDetailDTO> GetCategoryDetailById(Guid Id, string locale);
        public IDataResult<IQueryable<GetCategoryForSelectDTO>> GetAllCategoryForSelect(string locale);

    }
}
