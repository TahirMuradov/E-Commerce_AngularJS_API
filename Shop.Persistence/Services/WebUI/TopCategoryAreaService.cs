using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Application.Validators.WebUIValidators.TopCategoryAreaDTOValidations;
using Shop.Domain.Entities;
using Shop.Domain.Entities.WebUIEntites;
using Shop.Domain.Exceptions;
using Shop.Persistence.Context;
using System.Net;
using System.Threading.Tasks;

namespace Shop.Persistence.Services.WebUI
{
    public class TopCategoryAreaService : ITopCategoryAreaService
    {
        private readonly ILogger<TopCategoryAreaService> _logger;
        private readonly AppDBContext _context;
        private readonly IFileService _fileService;

        public TopCategoryAreaService(ILogger<TopCategoryAreaService> logger, AppDBContext context, IFileService fileService)
        {
            _logger = logger;
            _context = context;
            _fileService = fileService;
        }

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
        public async Task<IResult> AddTopCategoryAreaAsync(AddTopCategoryAreaDTO addTopCategoryAreaDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            AddTopCategoryAreaDTOValidation validation = new AddTopCategoryAreaDTOValidation(LangCode,SupportedLaunguages);
            var validationResult = await validation.ValidateAsync(addTopCategoryAreaDTO);
            if(!validationResult.IsValid)
            return new ErrorResult(messages: validationResult.Errors.Select(x=>x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            try
            {
                Category? category = await _context.Categories.AsNoTracking().Where(x=>x.Id==addTopCategoryAreaDTO.CategoryId).FirstOrDefaultAsync();
                TopCategoryArea topCategoryArea = new()
                {
                    IsActive=true,
                };
                if (category is not null)
                {
                    topCategoryArea.CategoryId = category.Id;
                }
                _context.TopCategoryAreas.Add(topCategoryArea);
                foreach (var langContent in addTopCategoryAreaDTO.Title)
                {

                    
                    TopCategoryAreaLanguage topCategoryAreaLanguage = new()
                    {
                        LangCode = langContent.Key,
                        Title = langContent.Value,
                        Description = addTopCategoryAreaDTO.Description[langContent.Key],
                        TopCategoryArea = topCategoryArea
                    };
                    _context.TopCategoryAreaLanguages.Add(topCategoryAreaLanguage);

                }
                await _context.SaveChangesAsync();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.Created);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }




        }

        public async Task<IDataResult<PaginatedList<GetTopCategoryAreaDTO>>> GetTopCategoryAreaByPageOrSearchAsync(string LangCode, int page, string? search = null)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<PaginatedList<GetTopCategoryAreaDTO>>(HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (page < 1)
                page = 1;
            IQueryable<GetTopCategoryAreaDTO> queryData = search is null? _context.TopCategoryAreas.AsNoTracking().AsSplitQuery().Select(x => new GetTopCategoryAreaDTO 
            {
                Id = x.Id,
                PictureUrl = x.ImageUrl,
                CategoryId = x.Category != null ? x.Category.Id.ToString() : null,
                Description = x.TopCategoryAreaLanguages.Where(lang => lang.LangCode == LangCode).Select(lang => lang.Description).FirstOrDefault(),
                Title = x.TopCategoryAreaLanguages.Where(lang => lang.LangCode == LangCode).Select(lang => lang.Title).FirstOrDefault()
                



            }):
            _context.TopCategoryAreas.AsNoTracking().AsSplitQuery().Select(x => new GetTopCategoryAreaDTO
            {
                Id = x.Id,
                PictureUrl = x.ImageUrl,
                CategoryId = x.Category != null ? x.Category.Id.ToString() : null,
                Description = x.TopCategoryAreaLanguages.Where(lang => lang.LangCode == LangCode).Select(lang => lang.Description).FirstOrDefault(),
                Title = x.TopCategoryAreaLanguages.Where(lang => lang.LangCode == LangCode).Select(lang => lang.Title).FirstOrDefault()




            }).Where(x=>x.Title.ToLower().Contains(search.ToLower()) ||
            x.Description.ToLower().Contains(search.ToLower()) ||
            x.Id.ToString().ToLower().Contains(search.ToLower()))
            ;

            var paginatedList = await PaginatedList<GetTopCategoryAreaDTO>.CreateAsync(queryData, page, 10);

            return new SuccessDataResult<PaginatedList<GetTopCategoryAreaDTO>>(paginatedList, HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
        }

        public IDataResult<IQueryable<GetTopCategoryAreaForUIDTO>> GetTopCategoryAreaForUI(string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode)||SupportedLaunguages.Contains(LangCode))

                return new ErrorDataResult<IQueryable<GetTopCategoryAreaForUIDTO>>(HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            IQueryable<GetTopCategoryAreaForUIDTO> queryData=_context.TopCategoryAreas.AsNoTracking().Where(x=>x.IsActive).Select(x=>  new GetTopCategoryAreaForUIDTO
            {
              
                PictureUrl = x.ImageUrl,
                CategoryId = x.Category != null ? x.Category.Id.ToString() : null,
                Description = x.TopCategoryAreaLanguages.Where(lang => lang.LangCode == LangCode).Select(lang => lang.Description).FirstOrDefault(),
                Title = x.TopCategoryAreaLanguages.Where(lang => lang.LangCode == LangCode).Select(lang => lang.Title).FirstOrDefault()
            });
            return new SuccessDataResult<IQueryable<GetTopCategoryAreaForUIDTO>>(queryData, HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
        }

        public async Task<IDataResult<GetTopCategoryAreaForUpdateDTO>> GetTopcategoryAreaForUpdateAsync(Guid Id, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || SupportedLaunguages.Contains(LangCode))

                return new ErrorDataResult<GetTopCategoryAreaForUpdateDTO>(HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (Id == default || Id == Guid.Empty)
                return new ErrorDataResult<GetTopCategoryAreaForUpdateDTO>(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.UnsupportedMediaType);
            GetTopCategoryAreaForUpdateDTO? topCategoryArea = await _context.TopCategoryAreas.AsNoTracking().Where(x=>x.Id==Id).Select(x=>new GetTopCategoryAreaForUpdateDTO
            {
                Id = x.Id,
                CategoryId = x.Category != null ? x.Category.Id.ToString() : null,
                PictureUrl = x.ImageUrl,
                Title = x.TopCategoryAreaLanguages.Where(lang => lang.LangCode == LangCode).Select(lang =>new KeyValuePair<string,string>(lang.LangCode,lang.Title)).ToDictionary(),
                Description = x.TopCategoryAreaLanguages.Where(lang => lang.LangCode == LangCode).Select(lang => new KeyValuePair<string, string>(lang.LangCode, lang.Description)).ToDictionary()
            }).FirstOrDefaultAsync();

            return topCategoryArea is not null
                ? new SuccessDataResult<GetTopCategoryAreaForUpdateDTO>(topCategoryArea, HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK)
                : new ErrorDataResult<GetTopCategoryAreaForUpdateDTO>(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);


        }

        public IResult ChangeVisibleTopCategoryArea(Guid Id, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || SupportedLaunguages.Contains(LangCode))

                return new ErrorDataResult<GetTopCategoryAreaForUpdateDTO>(HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (Id == default || Id == Guid.Empty)
                return new ErrorDataResult<GetTopCategoryAreaForUpdateDTO>(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.UnsupportedMediaType);

            try
            {
            TopCategoryArea? topCategoryArea = _context.TopCategoryAreas.FirstOrDefault(x=>x.Id==Id);
            if (topCategoryArea is null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            topCategoryArea.IsActive = false;
            _context.TopCategoryAreas.Update(topCategoryArea);
                _context.SaveChanges();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
        }
        public IResult RemoveTopCategoryArea(Guid Id, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || SupportedLaunguages.Contains(LangCode))

                return new ErrorDataResult<GetTopCategoryAreaForUpdateDTO>(HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (Id == default || Id == Guid.Empty)
                return new ErrorDataResult<GetTopCategoryAreaForUpdateDTO>(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.UnsupportedMediaType);

            try
            {
                TopCategoryArea? topCategoryArea = _context.TopCategoryAreas.FirstOrDefault(x => x.Id == Id);
                if (topCategoryArea is null)
                    return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
        var fileRemoveResult=  _fileService.RemoveFile(topCategoryArea.ImageUrl);
                if (!fileRemoveResult.IsSuccess)
                {
                    _context.TopCategoryAreas.Remove(topCategoryArea);
                    _context.SaveChanges();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
                }
                return new ErrorResult(message: fileRemoveResult.Message, fileRemoveResult.StatusCode);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
        }
        public async Task<IResult> UpdateTopCategoryAreaAsync(UpdateTopCategoryAreaDTO updateTopCategoryAreaDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);

            UpdateTopCategoryAreaDTOValidation validation = new UpdateTopCategoryAreaDTOValidation(LangCode, SupportedLaunguages);
            var validationResult = await validation.ValidateAsync(updateTopCategoryAreaDTO);

            if (!validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            try
            {
                TopCategoryArea? topCategoryArea = await _context.TopCategoryAreas.Include(x => x.TopCategoryAreaLanguages).FirstOrDefaultAsync(x => x.Id == updateTopCategoryAreaDTO.Id);
                if (topCategoryArea is null)
                    return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
                if (updateTopCategoryAreaDTO.NewImage is not null)
                {
                    var fileRemoveResult = _fileService.RemoveFile(topCategoryArea.ImageUrl);
                    if (!fileRemoveResult.IsSuccess)
                        return new ErrorResult(message: fileRemoveResult.Message, fileRemoveResult.StatusCode);
                    var saveFileResult = await _fileService.SaveFileAsync(updateTopCategoryAreaDTO.NewImage, false);
                    if (saveFileResult.IsSuccess)                                            
                    topCategoryArea.ImageUrl=saveFileResult.Data ;
                    else
                        return new ErrorResult(message: saveFileResult.Message, saveFileResult.StatusCode);

                }
           
                topCategoryArea.CategoryId = updateTopCategoryAreaDTO.CategoryId;
                topCategoryArea.IsActive = updateTopCategoryAreaDTO.IsActive;
                foreach (var langContent in updateTopCategoryAreaDTO.Title)
                {
                    TopCategoryAreaLanguage? topCategoryAreaLanguage = topCategoryArea.TopCategoryAreaLanguages.FirstOrDefault(x => x.LangCode == langContent.Key);
                    if (topCategoryAreaLanguage is not null)
                    {
                        topCategoryAreaLanguage.Title = langContent.Value;
                        topCategoryAreaLanguage.Description = updateTopCategoryAreaDTO.Description[langContent.Key];
                    }
                    else if(SupportedLaunguages.Contains(langContent.Key))
                    {
                        TopCategoryAreaLanguage newLanguage = new()
                        {
                            LangCode = langContent.Key,
                            Title = langContent.Value,
                            Description = updateTopCategoryAreaDTO.Description[langContent.Key],
                            TopCategoryAreaId = topCategoryArea.Id
                        };
                        _context.TopCategoryAreaLanguages.Add(newLanguage);
                    }
                }
                _context.TopCategoryAreas.Update(topCategoryArea);
                await _context.SaveChangesAsync();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }

        }
    }
}
