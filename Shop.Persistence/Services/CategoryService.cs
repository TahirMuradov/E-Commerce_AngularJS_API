using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Exceptions;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Domain.Entities;
using Shop.Persistence.Context;
using System.Net;

namespace Shop.Persistence.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDBContext _context;
        private readonly ILogger<CategoryService> _logger;
        private string[] SupportedLaunguages
        {
            get
            {



                return Configuration.config.GetSection("SupportedLanguage:Launguages").Get<string[]>();


            }
        }

        private string DefaultLaunguage
        {
            get
            {
                return Configuration.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        public CategoryService(AppDBContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IResult AddCategory(AddCategoryDTO addCategoryDTO, string locale)
        {
            try
            {

                var unsupportedLangs = addCategoryDTO.CategoryContent.Keys
         .Where(k => !SupportedLaunguages.Contains(k))
         .ToList();

                if (unsupportedLangs.Any())
                {
                    return new ErrorResult(
                        message: HttpStatusErrorMessages.UnsupportedLanguage[locale],
                        HttpStatusCode.BadRequest
                    );
                }
                Category category = new Category();
                _context.Categories.Add(category);


                foreach (var categoryContent in addCategoryDTO.CategoryContent)
                {
                    if (SupportedLaunguages.Contains(categoryContent.Key))
                    {
                        
                    CategoryLanguage categoryLang = new()
                    {
                        CategoryId = category.Id,
                        LanguageCode = categoryContent.Key,
                        Name = categoryContent.Value,

                    };
                    _context.CategoryLanguages.Add(categoryLang);
                    }


                }
                _context.SaveChanges();
                return new SuccessResult(message: HttpStatusErrorMessages.Created[locale], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: ex.Message, HttpStatusCode.BadRequest);
            }


        }

        public IResult DeleteCategory(Guid Id, string locale)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == Id);
            if (category is null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return new SuccessResult(message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public IDataResult<IQueryable<GetCategoryDTO>> GetAllCategory(string locale)
        {
            IQueryable<GetCategoryDTO> queryCategory = _context.Categories.AsNoTracking().Select(x => new GetCategoryDTO
            {
                CategoryId = x.Id,
                CategoryName = x.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == locale).Name
            });
            return new SuccessDataResult<IQueryable<GetCategoryDTO>>(data: queryCategory, message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public async Task<IDataResult<PaginatedList<GetCategoryDTO>>> GetAllCategoryByPageAsync(string locale, int page = 1)
        {
            IQueryable<GetCategoryDTO> queryCategory = _context.Categories.AsNoTracking().Select(x => new GetCategoryDTO
            {
                CategoryId = x.Id,
                CategoryName = x.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == locale).Name

            });
            var returnData = await PaginatedList<GetCategoryDTO>.CreateAsync(queryCategory, page, 10);
            return new SuccessDataResult<PaginatedList<GetCategoryDTO>>(data: returnData, message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public IDataResult<GetCategoryDTO> GetCategoryDetailById(Guid Id, string locale)
        {
            GetCategoryDTO category = _context.Categories.AsNoTracking()
                 .Where(x => x.Id == Id)
                 .Select(x => new GetCategoryDTO
                 {
                     CategoryId = x.Id,
                     CategoryName = x.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == locale).Name
                 }).FirstOrDefault();
            return category is null ?
                 new ErrorDataResult<GetCategoryDTO>(message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound) :
            new SuccessDataResult<GetCategoryDTO>(data: category, message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public IResult UpdateCategory(UpdateCategoryDTO updateCategoryDTO, string locale)
        {
            var category = _context.Categories.Include(x => x.CategoryLanguages).FirstOrDefault(x => x.Id == updateCategoryDTO.Id);
            if (category is null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
            foreach (var newContent in updateCategoryDTO.CategoryContent)
            {
                CategoryLanguage categoryLanguage = category.CategoryLanguages.FirstOrDefault(x => x.LanguageCode == newContent.Key);
                if (categoryLanguage.Name != newContent.Value)
                {
                    categoryLanguage.Name = newContent.Value;
                    _context.CategoryLanguages.Update(categoryLanguage);
                }
                else
                    continue;


            }
            _context.SaveChanges();
            return new SuccessResult(message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }
    }
}
