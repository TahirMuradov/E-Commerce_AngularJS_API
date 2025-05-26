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
        public Task<IDataResult<PaginatedList<GetCategoryDTO>>> GetAllCategoryByPageAsync(string locale, int page = 1);
        public IDataResult<IQueryable<GetCategoryDTO>> GetAllCategory(string locale);
        public IDataResult<GetCategoryDTO> GetCategoryDetailById(Guid Id, string locale);

    }
}
