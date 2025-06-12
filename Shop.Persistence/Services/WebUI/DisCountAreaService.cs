using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI.DisCountAreaDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Application.Validators.WebUIValidators.DisCountAreaDTOValidations;
using Shop.Domain.Entities.WebUIEntites;
using Shop.Domain.Exceptions;
using Shop.Persistence.Context;
using System.Net;

namespace Shop.Persistence.Services.WebUI
{
    public class DisCountAreaService : IDiscountAreaService
    {
        private readonly ILogger<DisCountAreaService> _logger;
        private readonly AppDBContext _context;
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
        public DisCountAreaService(ILogger<DisCountAreaService> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IResult AddDiscountArea(AddDisCountAreaDTO addDisCountAreaDTO, string langCode)
        {
            if (string.IsNullOrEmpty(langCode) || !SupportedLaunguages.Contains(langCode))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            AddDiscountAreaDTOValidation validationRules = new AddDiscountAreaDTOValidation(langCode, SupportedLaunguages);
            var validationResult = validationRules.Validate(addDisCountAreaDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            try
            {
                DisCountArea disCountArea = new DisCountArea();
                _context.DisCountAreas.Add(disCountArea);
                foreach (var newContent in addDisCountAreaDTO.TitleContent)
                {
                    DisCountAreaLanguage disCountAreaLanguage = new DisCountAreaLanguage
                    {
                        Title = newContent.Value,
                        Description = addDisCountAreaDTO.DescriptionContent[newContent.Key],
                        LangCode = newContent.Key,
                        DisCountAreaId = disCountArea.Id

                    };
                    _context.DisCountAreaLanguages.Add(disCountAreaLanguage);

                }
                _context.SaveChanges();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[langCode], HttpStatusCode.Created);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[langCode], HttpStatusCode.InternalServerError);
            }
        }


        public IResult Delete(Guid Id, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))

                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (Id == default || Id == Guid.Empty)
                return new ErrorResult(message: HttpStatusErrorMessages.NotFound[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            try
            {
                DisCountArea disCountArea = _context.DisCountAreas.FirstOrDefault(x => x.Id == Id);
                if (disCountArea is null)
                    return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);
                _context.DisCountAreas.Remove(disCountArea);
                _context.SaveChanges();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: HttpStatusErrorMessages.InternalServerError[LangCode], HttpStatusCode.InternalServerError);
                throw;
            }

        }

        public IDataResult<IQueryable<GetDisCountAreaDTO>> GetAllDisCountArea(string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorDataResult<IQueryable<GetDisCountAreaDTO>>(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            IQueryable<GetDisCountAreaDTO> disCountAreas = _context.DisCountAreas.AsNoTracking()
                .Select(x => new GetDisCountAreaDTO
                {
                    Id = x.Id,
                  Description=x.Languages.Where(y => y.LangCode == LangCode).Select(y => y.Description).FirstOrDefault(),
                    Title = x.Languages.Where(y => y.LangCode == LangCode).Select(y => y.Title).FirstOrDefault()
                });

            return new SuccessDataResult<IQueryable<GetDisCountAreaDTO>>(disCountAreas, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);
        }

        public async Task<IDataResult<PaginatedList<GetDisCountAreaDTO>>> GetAllDisCountByPageOrSearchAsync(string LangCode, int page, string? search = null)
        {
            if (string.IsNullOrEmpty(LangCode)||!SupportedLaunguages.Contains(LangCode))  
        return new ErrorDataResult<PaginatedList<GetDisCountAreaDTO>>(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            IQueryable<GetDisCountAreaDTO> disCountAreas = search is null? _context.DisCountAreas.AsNoTracking().AsSplitQuery()
                .Select(x => new GetDisCountAreaDTO
                {
                    Id = x.Id,
                    Description = x.Languages.Where(y => y.LangCode == LangCode).Select(y => y.Description).FirstOrDefault(),
                    Title = x.Languages.Where(y => y.LangCode == LangCode).Select(y => y.Title).FirstOrDefault()
                }):
                _context.DisCountAreas.AsNoTracking().AsSplitQuery()
                .Select(x => new GetDisCountAreaDTO
                {
                    Id = x.Id,
                    Description = x.Languages.Where(y => y.LangCode == LangCode).Select(y => y.Description).FirstOrDefault(),
                    Title = x.Languages.Where(y => y.LangCode == LangCode).Select(y => y.Title).FirstOrDefault()
                }).Where(x=>x.Title.ToLower().Contains(search.ToLower())||
                x.Description.ToLower().Contains(search.ToLower())
                || x.Id.ToString().ToLower().Contains(search.ToLower())
                )
                ;
            PaginatedList<GetDisCountAreaDTO> paginatedList = await PaginatedList<GetDisCountAreaDTO>.CreateAsync(disCountAreas, page, 10);
            return new SuccessDataResult<PaginatedList<GetDisCountAreaDTO>>(paginatedList, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

        }

        public async Task<IDataResult<GetDisCountAreaForUpdateDTO>> GetDisCountAreaForUpdateAsync(Guid Id, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
          return new ErrorDataResult<GetDisCountAreaForUpdateDTO>(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (Id == default || Id == Guid.Empty)
                return new ErrorDataResult<GetDisCountAreaForUpdateDTO>(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.UnsupportedMediaType);
            GetDisCountAreaForUpdateDTO? getDisCountAreaForUpdateDTO = await _context.DisCountAreas.AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => new GetDisCountAreaForUpdateDTO
                {
                    Id = x.Id,
                    TitleContent = x.Languages.Select(kv=>new KeyValuePair<string,string>(kv.LangCode,kv.Title)).ToDictionary(),
                    DescriptionContent = x.Languages.Select(kv => new KeyValuePair<string, string>(kv.LangCode, kv.Description)).ToDictionary()
                }).FirstOrDefaultAsync();
            return getDisCountAreaForUpdateDTO is null
                ? new ErrorDataResult<GetDisCountAreaForUpdateDTO>(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound)
                : new SuccessDataResult<GetDisCountAreaForUpdateDTO>(getDisCountAreaForUpdateDTO, message: HttpStatusErrorMessages.Success[LangCode], HttpStatusCode.OK);

        }

        public IResult UpdateDisCountArea(UpdateDisCountAreaDTO updateDisCountAreaDTO, string LangCode)
        {
            if (string.IsNullOrEmpty(LangCode) || !SupportedLaunguages.Contains(LangCode))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);

            UpdateDisCountAreaDTOValidation validationRules = new UpdateDisCountAreaDTOValidation(LangCode, SupportedLaunguages);
            var validationResult = validationRules.Validate(updateDisCountAreaDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            try
            {
                DisCountArea? disCountArea = _context.DisCountAreas.Include(x => x.Languages).FirstOrDefault(x => x.Id == updateDisCountAreaDTO.Id);
                if (disCountArea is null)
                    return new ErrorResult(message: HttpStatusErrorMessages.NotFound[LangCode], HttpStatusCode.NotFound);

                foreach (var newLang in updateDisCountAreaDTO.TitleContent)
                {
                    DisCountAreaLanguage? disCountAreaLanguage = disCountArea.Languages.FirstOrDefault(x => x.LangCode == newLang.Key);
                    if (disCountAreaLanguage is not null)
                    {
                        disCountAreaLanguage.Title = newLang.Value;
                        disCountAreaLanguage.Description = updateDisCountAreaDTO.DescriptionContent[newLang.Key];
                        _context.DisCountAreaLanguages.Update(disCountAreaLanguage);
                    }
                    else if (SupportedLaunguages.Contains(newLang.Key))
                    {
                        DisCountAreaLanguage newDisCountAreaLanguage = new DisCountAreaLanguage
                        {
                            Title = newLang.Value,
                            Description = updateDisCountAreaDTO.DescriptionContent[newLang.Key],
                            LangCode = newLang.Key,
                            DisCountAreaId = disCountArea.Id
                        };
                        _context.DisCountAreaLanguages.Add(newDisCountAreaLanguage);

                    }
            }
                    _context.SaveChanges();
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
