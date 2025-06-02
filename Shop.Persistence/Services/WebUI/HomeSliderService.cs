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
        public async Task<IResult> AddHomeSliderItemAsync(AddHomeSliderItemDTO addHomeSliderItemDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[LangCode], HttpStatusCode.BadRequest);

            AddHomeSliderItemDTOValidation validationRules = new AddHomeSliderItemDTOValidation(LangCode, SupportedLaunguages);
            var validationResult = await validationRules.ValidateAsync(addHomeSliderItemDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
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

              var resultFile=   await    _fileService.SaveFileAsync(addHomeSliderItemDTO.BackgroundImage, false);
                if (!resultFile.IsSuccess)
                    return new ErrorResult(messages: resultFile.Messages, HttpStatusCode.BadRequest);
                newSliderItem.BackgroundImageUrl = resultFile.Data;
                _context.Update(newSliderItem);
                await _context.SaveChangesAsync();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
           


        }

        public IResult DeleteHomeSliderItem(Guid Id, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[LangCode], HttpStatusCode.BadRequest);
            if (Id==default||Guid.Empty==Id)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.BadRequest);
            HomeSliderItem? homeSliderItem = _context.HomeSliderItems.FirstOrDefault(x => x.Id == Id);
            if (homeSliderItem == null)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            try
            {
                _context.HomeSliderItems.Remove(homeSliderItem);
                _context.SaveChanges();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[LangCode],HttpStatusCode.InternalServerError);
            }

        }

        public async Task<IDataResult<PaginatedList<GetHomeSliderItemDTO>>> GetAllHomeSliderAsync(string LangCode, int page)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<PaginatedList<GetHomeSliderItemDTO>> (message: HttpStatusErrorMessages.UnsupportedLanguage[LangCode], HttpStatusCode.BadRequest);
            if (page < 1)
                page = 1;
            try
            {
                IQueryable<GetHomeSliderItemDTO> queryData = _context.HomeSliderItems.AsNoTracking().AsSplitQuery().Select(x => new GetHomeSliderItemDTO
                {
                    Id = x.Id,
                    ImageUrl=x.BackgroundImageUrl,
                    Description = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Description).FirstOrDefault(),
                    Title = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Title).FirstOrDefault(),
                });
                PaginatedList<GetHomeSliderItemDTO> paginatedList = await PaginatedList<GetHomeSliderItemDTO>.CreateAsync(queryData, page, 10);
                return new SuccessDataResult<PaginatedList<GetHomeSliderItemDTO>>(data: paginatedList, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<PaginatedList<GetHomeSliderItemDTO>>(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
        }

        public IDataResult<IQueryable<GetHomeSliderItemForUIDTO>> GetHomeSliderItemForUI(string LangCode)
        {

            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<IQueryable<GetHomeSliderItemForUIDTO>>(message: HttpStatusErrorMessages.UnsupportedLanguage[LangCode], HttpStatusCode.BadRequest);
            try
            {
                IQueryable<GetHomeSliderItemForUIDTO> queryData = _context.HomeSliderItems.AsNoTracking().AsSplitQuery().Select(x => new GetHomeSliderItemForUIDTO
                {
          
                    ImageUrl = x.BackgroundImageUrl,
                    Description = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Description).FirstOrDefault(),
                    Title = x.Languages.Where(y => y.LangCode == LangCode).Select(s => s.Title).FirstOrDefault(),
                });
                return new SuccessDataResult<IQueryable<GetHomeSliderItemForUIDTO>>(data: queryData, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorDataResult<IQueryable<GetHomeSliderItemForUIDTO>>(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
            }
        }

        public async Task<IDataResult<GetHomeSliderItemForUpdateDTO>> GetHomeSliderItemForUpdateAsync(Guid Id, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<GetHomeSliderItemForUpdateDTO>(message: HttpStatusErrorMessages.UnsupportedLanguage[LangCode], HttpStatusCode.BadRequest);
            if (Id == default || Guid.Empty == Id)
                return new ErrorDataResult<GetHomeSliderItemForUpdateDTO>(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.BadRequest);
            GetHomeSliderItemForUpdateDTO? getHomeSliderItemForUpdateDTO= await _context.HomeSliderItems.AsNoTracking().AsSplitQuery().Where(x=>x.Id==Id).Select(x => new GetHomeSliderItemForUpdateDTO
            {
                Id = x.Id,
                ImageUrl = x.BackgroundImageUrl,
                Title = x.Languages.Where(y => y.LangCode == LangCode).ToDictionary(s => s.LangCode, s => s.Title),
                Description = x.Languages.Where(y => y.LangCode == LangCode).ToDictionary(s => s.LangCode, s => s.Description),
            }).FirstOrDefaultAsync();
            if (getHomeSliderItemForUpdateDTO is null)
                return new ErrorDataResult<GetHomeSliderItemForUpdateDTO>(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
            return new SuccessDataResult<GetHomeSliderItemForUpdateDTO>(data: getHomeSliderItemForUpdateDTO, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
        }

        public async Task<IResult> UpdateHomeSliderItemAsync(UpdateHomeSliderItemDTO updateHomeSliderItemDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[LangCode], HttpStatusCode.BadRequest);
            UpdateHomeSliderItemDTOValidation validationRules = new UpdateHomeSliderItemDTOValidation(LangCode, SupportedLaunguages);
            var validationResult = await validationRules.ValidateAsync(updateHomeSliderItemDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            try
            {
                HomeSliderItem? homeSliderItem=_context.HomeSliderItems.Include(x => x.Languages).FirstOrDefault(x => x.Id == updateHomeSliderItemDTO.Id);
                if (homeSliderItem is null)
                    return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
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
                            HomeSliderItemId = homeSliderItem.Id
                        };
                        _context.HomeSliderLanguages.Add(homeSliderLanguage);
                    }
                }
                if (updateHomeSliderItemDTO.NewImage is not null)
                {
                    var fileResult = await _fileService.SaveFileAsync(updateHomeSliderItemDTO.NewImage, false);
                    if (!fileResult.IsSuccess)
                        return new ErrorResult(messages: fileResult.Messages, HttpStatusCode.BadRequest);
                    var deleteOldFileResult =  _fileService.RemoveFile(homeSliderItem.BackgroundImageUrl);
                    if (!deleteOldFileResult.IsSuccess)
                        return new ErrorResult(messages: deleteOldFileResult.Messages, HttpStatusCode.BadRequest);
                    homeSliderItem.BackgroundImageUrl = fileResult.Data;
                    _context.HomeSliderItems.Update(homeSliderItem);

                }
             await   _context.SaveChangesAsync();
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
