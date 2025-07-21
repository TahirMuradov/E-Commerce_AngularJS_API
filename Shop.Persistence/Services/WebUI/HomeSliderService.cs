using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI.HomeSliderItemDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Application.Validators.WebUIValidators.HomeSliderItemDTOValidations;
using Shop.Domain.Entities;
using Shop.Domain.Entities.WebUIEntites;
using Shop.Domain.Exceptions;
using Shop.Persistence.Context;
using System.Net;

namespace Shop.Persistence.Services.WebUI
{
    public class HomeSliderService : IHomeSliderService
    {
        private readonly ILogger<HomeSliderService> _logger;
        private readonly AppDBContext _context;
        private readonly IFileService _fileService;

        public HomeSliderService(ILogger<HomeSliderService> logger, AppDBContext context, IFileService fileServic)
        {
            _logger = logger;
            _context = context;
            _fileService = fileServic;
        }

        private string[] SupportedLaunguages
        {
            get
            {



                return ConfigurationPersistence.SupportedLaunguageKeys;


            }
        }

        private string DefaultLaunguage
        {
            get
            {
                return ConfigurationPersistence.DefaultLanguageKey;
            }
        }
        public async Task<IResult> AddHomeSliderItemAsync(AddHomeSliderItemDTO addHomeSliderItemDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);

            AddHomeSliderItemDTOValidation validationRules = new AddHomeSliderItemDTOValidation(SupportedLaunguages);
            var validationResult = await validationRules.ValidateAsync(addHomeSliderItemDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(LangCode,messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            try
            {
                HomeSliderItem newSliderItem = new HomeSliderItem();
                _context.HomeSliderItems.Add(newSliderItem);
                foreach (var langContent in addHomeSliderItemDTO.Title)
                {
                    HomeSliderLanguage homeSliderLanguage = new()
                    {
                        Description = addHomeSliderItemDTO.Description[langContent.Key],
                        Title = langContent.Value,
                        LangCode = langContent.Key,
                        HomeSliderItemId = newSliderItem.Id

                    };
                    _context.HomeSliderLanguages.Add(homeSliderLanguage);
                }

              var resultFile=   await    _fileService.SaveImageAsync(LangCode,addHomeSliderItemDTO.BackgroundImage, false);
                if (!resultFile.IsSuccess)
                    return new ErrorResult(LangCode,messages: resultFile.Messages, HttpStatusCode.BadRequest);
              Image ımage = new Image
              {
                  Path = resultFile.Data,
                  HomeSliderItemId = newSliderItem.Id

              };
     _context.Images.Add(ımage);
                await _context.SaveChangesAsync();
                return new SuccessResult(LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
           


        }

        public IResult DeleteHomeSliderItem(Guid Id, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);
            if (Id==default||Guid.Empty==Id)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.BadRequest);
            HomeSliderItem? homeSliderItem = _context.HomeSliderItems.Include(x=>x.Image).FirstOrDefault(x => x.Id == Id);
            if (homeSliderItem == null)
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);

           IResult fileRemoveResult= _fileService.RemoveFile(LangCode,homeSliderItem.Image.Path);
            if (!fileRemoveResult.IsSuccess)
                return fileRemoveResult;

            try
            {
                _context.HomeSliderItems.Remove(homeSliderItem);
                _context.SaveChanges();
                return new SuccessResult(LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.InternalServerError[LangCode],HttpStatusCode.InternalServerError);
            }

        }

        public async Task<IDataResult<PaginatedList<GetHomeSliderItemDTO>>> GetAllHomeSliderByPageOrSearchAsync(string LangCode, int page,string? search=null)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<PaginatedList<GetHomeSliderItemDTO>> (DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);
            if (page < 1)
                page = 1;
            try
            {
                IQueryable<GetHomeSliderItemDTO> queryData = search is null? _context.HomeSliderItems.AsNoTracking().AsSplitQuery().Select(x => new GetHomeSliderItemDTO
                {
                    Id = x.Id,
                    ImageUrl=x.Image.Path,
                    Description = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Description).FirstOrDefault(),
                    Title = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Title).FirstOrDefault(),
                }):
                _context.HomeSliderItems.AsNoTracking().AsSplitQuery().Select(x => new GetHomeSliderItemDTO
                {
                    Id = x.Id,
                    ImageUrl = x.Image.Path,
                    Description = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Description).FirstOrDefault(),
                    Title = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Title).FirstOrDefault(),
                }).Where(x=>x.Title.ToLower().Contains(search.ToLower())||
                x.Description.ToLower().Contains(search.ToLower()) ||
                x.Id.ToString().ToLower().Contains(search.ToLower()))
                ;
                PaginatedList<GetHomeSliderItemDTO> paginatedList = await PaginatedList<GetHomeSliderItemDTO>.CreateAsync(queryData, page, 10);
                return new SuccessDataResult<PaginatedList<GetHomeSliderItemDTO>>(data: paginatedList, LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<PaginatedList<GetHomeSliderItemDTO>>(LangCode,message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
        }

        public IDataResult<IQueryable<GetHomeSliderItemForUIDTO>> GetHomeSliderItemForUI(string LangCode)
        {

            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<IQueryable<GetHomeSliderItemForUIDTO>>(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);
            try
            {
                IQueryable<GetHomeSliderItemForUIDTO> queryData = _context.HomeSliderItems.AsNoTracking().AsSplitQuery().Select(x => new GetHomeSliderItemForUIDTO
                {
          
                    ImageUrl = x.Image.Path,
                    Description = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Description).FirstOrDefault(),
                    Title = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Title).FirstOrDefault(),
                });
                return new SuccessDataResult<IQueryable<GetHomeSliderItemForUIDTO>>(data: queryData, LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<IQueryable<GetHomeSliderItemForUIDTO>>(LangCode,message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
        }

        public async Task<IDataResult<GetHomeSliderItemForUpdateDTO>> GetHomeSliderItemForUpdateAsync(Guid Id, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<GetHomeSliderItemForUpdateDTO>(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);
            if (Id == default || Guid.Empty == Id)
                return new ErrorDataResult<GetHomeSliderItemForUpdateDTO>(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.BadRequest);
            GetHomeSliderItemForUpdateDTO? getHomeSliderItemForUpdateDTO= await _context.HomeSliderItems.AsNoTracking().Where(x=>x.Id==Id).Select(x => new GetHomeSliderItemForUpdateDTO
            {
                Id = x.Id,
                ImageUrl = x.Image.Path,
                Title = x.Languages.Select(x=>new KeyValuePair<string,string>(x.LangCode,x.Title)).ToDictionary(),
                Description = x.Languages.Select(x=>new KeyValuePair<string,string>(x.LangCode,x.Description)).ToDictionary(),
            }).FirstOrDefaultAsync();
            if (getHomeSliderItemForUpdateDTO is null)
                return new ErrorDataResult<GetHomeSliderItemForUpdateDTO>(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            return new SuccessDataResult<GetHomeSliderItemForUpdateDTO>(data: getHomeSliderItemForUpdateDTO,LangCode, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
        }

        public async Task<IResult> UpdateHomeSliderItemAsync(UpdateHomeSliderItemDTO updateHomeSliderItemDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.BadRequest);
            UpdateHomeSliderItemDTOValidation validationRules = new UpdateHomeSliderItemDTOValidation(SupportedLaunguages);
            var validationResult = await validationRules.ValidateAsync(updateHomeSliderItemDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(LangCode,messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            try
            {
                HomeSliderItem? homeSliderItem=_context.HomeSliderItems.Include(x => x.Languages).Include(x=>x.Image).FirstOrDefault(x => x.Id == updateHomeSliderItemDTO.Id);
                if (homeSliderItem is null)
                    return new ErrorResult(LangCode,message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
                foreach (var content in updateHomeSliderItemDTO.Title)
                {
                    HomeSliderLanguage? homeSliderLanguage = homeSliderItem.Languages.FirstOrDefault(x => x.LangCode == content.Key);
                    if (homeSliderLanguage is not null)
                    {
                        homeSliderLanguage.Title = content.Value;
                        homeSliderLanguage.Description = updateHomeSliderItemDTO.Description[content.Key];
                        _context.HomeSliderLanguages.Update(homeSliderLanguage);

                    }
                    else
                    {
                        HomeSliderLanguage newhomeSliderLanguage = new HomeSliderLanguage
                        {
                            LangCode = content.Key,
                            Description = updateHomeSliderItemDTO.Description[content.Key],
                            Title = content.Value,                            
                            HomeSliderItemId = homeSliderItem.Id,
                            HomeSliderItem=homeSliderItem
                        };
                        _context.HomeSliderLanguages.Add(homeSliderLanguage);
                    }
                }
                if (updateHomeSliderItemDTO.NewImage is not null)
                {
                    var fileResult = await _fileService.SaveImageAsync(LangCode,updateHomeSliderItemDTO.NewImage, false);
                    if (!fileResult.IsSuccess)
                        return new ErrorResult(LangCode,messages: fileResult.Messages, HttpStatusCode.BadRequest);

                    var deleteOldFileResult =  _fileService.RemoveFile(LangCode,homeSliderItem.Image.Path);
                    if (!deleteOldFileResult.IsSuccess)
                        return new ErrorResult(LangCode,messages: deleteOldFileResult.Messages, HttpStatusCode.BadRequest);
                 homeSliderItem.Image.Path = fileResult.Data;

                  
                
                }
                _context.HomeSliderItems.Update(homeSliderItem);
                await   _context.SaveChangesAsync();
                return new SuccessResult(LangCode,message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(LangCode,message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
                    
        }
    }
}
