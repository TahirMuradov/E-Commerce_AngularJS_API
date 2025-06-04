using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Domain.Exceptions;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Application.Validators.CategoryValidations;
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
                if (string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                    return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);

                AddCategoryDTOValidation validationRules = new AddCategoryDTOValidation(locale, SupportedLaunguages);
                var validationResult = validationRules.Validate(addCategoryDTO);
                if (!validationResult.IsValid)
                    return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            try
            {

                Category category = new Category()
                {
                    IsFeatured = addCategoryDTO.IsFeatured,
                };
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
            if (Id == default || string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            Category category = _context.Categories.FirstOrDefault(x => x.Id == Id);
            if (category is null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return new SuccessResult(message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public IDataResult<IQueryable<GetCategoryDTO>> GetAllCategory(string locale)
        {
            if (string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<IQueryable<GetCategoryDTO>>(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            IQueryable<GetCategoryDTO> queryCategory = _context.Categories.AsNoTracking().Select(x => new GetCategoryDTO
            {
                Id = x.Id,
                CategoryName = x.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == locale).Name,
                IsFEatured=x.IsFeatured
            });
            return new SuccessDataResult<IQueryable<GetCategoryDTO>>(data: queryCategory, message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public async Task<IDataResult<PaginatedList<GetCategoryDTO>>> GetAllCategoryByPageOrSearchAsync(string locale, int page = 1, string? search = null)
        {
            if ( string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<PaginatedList<GetCategoryDTO>>(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (page < 1)
                page = 1;
            IQueryable<GetCategoryDTO> queryCategory =   search is null? _context.Categories.AsNoTracking().AsSplitQuery().Select(x => new GetCategoryDTO
            {
                Id = x.Id,
                CategoryName = x.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == locale).Name,
                IsFEatured = x.IsFeatured

            }):
            _context.Categories.AsNoTracking().AsSplitQuery().Select(x => new GetCategoryDTO
            {
                Id = x.Id,
                CategoryName = x.CategoryLanguages.FirstOrDefault(y => y.LanguageCode == locale).Name,
                IsFEatured = x.IsFeatured

            }).Where(x=>x.CategoryName.ToLower().Contains(search.ToLower())||
            x.Id.ToString().ToLower().Contains(search.ToLower())
            )
            ;
            var returnData = await PaginatedList<GetCategoryDTO>.CreateAsync(queryCategory, page, 1);
            return new SuccessDataResult<PaginatedList<GetCategoryDTO>>(data: returnData, message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public IDataResult<GetCategoryDetailDTO> GetCategoryDetailById(Guid Id, string locale)
        {
            if (Id == default || string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<GetCategoryDetailDTO>(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            GetCategoryDetailDTO category = _context.Categories.AsNoTracking()
                 .Where(x => x.Id == Id)
                 .Select(x => new GetCategoryDetailDTO
                 {
                     Id = x.Id,
                     CategoryContent = x.CategoryLanguages.Select(x=>new KeyValuePair<string,string>(x.LanguageCode,x.Name)).ToDictionary(),
                     IsFeatured=x.IsFeatured
                 }).FirstOrDefault();
            return category is null ?
                 new ErrorDataResult<GetCategoryDetailDTO>(message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound) :
            new SuccessDataResult<GetCategoryDetailDTO>(data: category, message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public IResult UpdateCategory(UpdateCategoryDTO updateCategoryDTO, string locale)

        {
            if (string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            UpdateCategoryDTOValidation validationRules = new UpdateCategoryDTOValidation(locale, SupportedLaunguages);
            var validationResult = validationRules.Validate(updateCategoryDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            var category = _context.Categories.Include(x => x.CategoryLanguages).FirstOrDefault(x => x.Id == updateCategoryDTO.Id);
            if (category is null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
            category.IsFeatured = updateCategoryDTO.IsFeatured;
            foreach (var newContent in updateCategoryDTO.CategoryContent)
            {
                CategoryLanguage categoryLanguage = category.CategoryLanguages.FirstOrDefault(x => x.LanguageCode == newContent.Key);
                if (categoryLanguage.LanguageCode != newContent.Value)
                {
                    categoryLanguage.Name = newContent.Value;
                    _context.CategoryLanguages.Update(categoryLanguage);
                }
                else
                {
                    categoryLanguage = new CategoryLanguage
                    {
                        CategoryId = category.Id,
                        LanguageCode = newContent.Key,
                        Name = newContent.Value
                    };
                    _context.CategoryLanguages.Add(categoryLanguage);
                }


            }
            _context.SaveChanges();
            return new SuccessResult(message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }
    }
}
