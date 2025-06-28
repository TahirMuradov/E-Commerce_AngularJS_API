using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.ShippingMethodDTOs;
using Shop.Domain.Exceptions;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Domain.Entities;
using Shop.Persistence.Context;
using System.Net;

namespace Shop.Persistence.Services
{
    public class ShippingMethodService : IShippingMethodService
    {
        private readonly AppDBContext _context;
        private readonly ILogger<ShippingMethodService> _logger;
        private string[] SupportedLaunguages
        {
            get
            {



                return Configuration.SupportedLaunguageKeys;


            }
        }

        private string DefaultLaunguage
        {
            get
            {
                return Configuration.DefaultLanguageKey;
            }
        }
        public ShippingMethodService(AppDBContext context, ILogger<ShippingMethodService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IResult AddShippingMethod(AddShippingMethodDTO addShippingMethodDTO, string locale)
        {
            if (string.IsNullOrEmpty(locale)||!SupportedLaunguages.Contains(locale))
           return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
           
            try
            {
                ShippingMethod shippingMethod = new ShippingMethod
                {
                    DisCountPrice = addShippingMethodDTO.DisCountPrice,
                    Price = addShippingMethodDTO.Price,

                };
                _context.ShippingMethods.Add(shippingMethod);

                foreach (var methodLang in addShippingMethodDTO.Content)
                {
                    ShippingMethodLanguage shippingMethodLanguage = new()
                    {
                        LangCode = methodLang.Key,
                        Content = methodLang.Value,
                        ShippingMethodId = shippingMethod.Id
                    };
                    _context.ShippingMethodLanguages.Add(shippingMethodLanguage);
                }

                _context.SaveChanges();
                return new SuccessResult(message: HttpStatusErrorMessages.Created[locale] , HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public IResult DeleteShippingMethod(Guid id, string locale)
        {
            if (id==default||string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            try {
            
            ShippingMethod? shippingMethod = _context.ShippingMethods.FirstOrDefault(x=>x.Id==id);
                if (shippingMethod == null)
                    return new ErrorResult(message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
                _context.ShippingMethods.Remove(shippingMethod);
                _context.SaveChanges();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: ex.Message, HttpStatusCode.InternalServerError);
            }

        }

        public IDataResult<IQueryable<GetShippingMethodDTO>> GetAllShippingMethods(string locale)
        {
            if (string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<IQueryable<GetShippingMethodDTO>>(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
        return new SuccessDataResult<IQueryable<GetShippingMethodDTO>>(data:_context.ShippingMethods
                .Select(x => new GetShippingMethodDTO
                {
                    Id = x.Id,
                    Price = x.Price,
                    DisCount = x.DisCountPrice,
                    content = x.ShippingMethodLanguages.FirstOrDefault(y => y.LangCode == locale).Content
                }).AsNoTracking(), message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);

        }

        public async Task<IDataResult<PaginatedList<GetShippingMethodDTO>>> GetAllShippingMethodsByPageOrSearchAsync(int page, string locale, string? search = null)
        {
            if ( string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<PaginatedList<GetShippingMethodDTO>>(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);

            if (page < 1)
                page = 1;

                 IQueryable<GetShippingMethodDTO> queryShippingMethod = search is null? _context.ShippingMethods.Select(x => new GetShippingMethodDTO {

                     content = x.ShippingMethodLanguages.FirstOrDefault(y => y.LangCode == locale).Content,
                     DisCount = x.DisCountPrice,
                     Id = x.Id,
                     Price = x.Price



                 }).AsNoTracking().AsSplitQuery():
                 _context.ShippingMethods.Select(x => new GetShippingMethodDTO
                 {

                     content = x.ShippingMethodLanguages.FirstOrDefault(y => y.LangCode == locale).Content,
                     DisCount = x.DisCountPrice,
                     Id = x.Id,
                     Price = x.Price



                 }).AsNoTracking().AsSplitQuery().Where(x => x.content.ToLower().Contains(search.ToLower())||
                 x.Price.ToString().Contains(search.ToLower()) ||
                    x.DisCount.ToString().Contains(search.ToLower())||
                    x.Id.ToString().ToLower().Contains(search.ToLower())
                    )
                 ;
            PaginatedList<GetShippingMethodDTO>paginatedData =await PaginatedList<GetShippingMethodDTO>.CreateAsync(queryShippingMethod, page, 10);
            return new SuccessDataResult<PaginatedList<GetShippingMethodDTO>>(data: paginatedData, message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);


        }

        public IDataResult<GetShippingMethodDetailDTO> GetShippingMethodById(Guid id, string locale)
        {
            if (id==default ||string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<GetShippingMethodDetailDTO>(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
       GetShippingMethodDetailDTO? getShippingMethodDTO =_context.ShippingMethods.Where(x=>x.Id==id)
                .Select(x => new GetShippingMethodDetailDTO{
                    Id = x.Id,
                    Price = x.Price,
                    DisCount = x.DisCountPrice,
                    content = x.ShippingMethodLanguages.Select(y=> new KeyValuePair<string,string>(y.LangCode,y.Content)).ToDictionary()
                }).AsNoTracking().FirstOrDefault();
            return new SuccessDataResult<GetShippingMethodDetailDTO>(data: getShippingMethodDTO, message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);

        }

        public IResult UdpateShippingMethod(UpdateShippingMethodDTO updateShippingMethodDTO, string locale)
        {
            if (updateShippingMethodDTO.Id==default ||string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorResult(message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            try { 
            ShippingMethod? shippingMethod=_context.ShippingMethods.Include(x=>x.ShippingMethodLanguages).FirstOrDefault(x => x.Id == updateShippingMethodDTO.Id);
                if (shippingMethod is null)
                    return new ErrorResult(message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
                shippingMethod.DisCountPrice = updateShippingMethodDTO.DisCountPrice;
                shippingMethod.Price = updateShippingMethodDTO.Price;
                foreach (var methodLang in updateShippingMethodDTO.Content)
                {
                    ShippingMethodLanguage? shippingMethodLanguage = shippingMethod.ShippingMethodLanguages.FirstOrDefault(x => x.LangCode == methodLang.Key);
                    if (shippingMethodLanguage is not null)
                    {
                        shippingMethodLanguage.Content = methodLang.Value;
                    }
                    else 
                    {
                        shippingMethodLanguage = new ShippingMethodLanguage
                        {
                            LangCode = methodLang.Key,
                            Content = methodLang.Value,
                            ShippingMethodId = shippingMethod.Id
                        };
                        _context.ShippingMethodLanguages.Add(shippingMethodLanguage);
                    }
                    _context.ShippingMethodLanguages.Update(shippingMethodLanguage);
                }
                _context.SaveChanges();
                return new SuccessResult(message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);


            } catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(message: ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
