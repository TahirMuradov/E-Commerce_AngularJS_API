
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.PaymentMethodDTOs;
using Shop.Domain.Exceptions;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Application.Validators.PaymentMethodValidations;
using Shop.Domain.Entities;
using Shop.Persistence.Context;
using System.Net;
using System.Threading.Tasks;

namespace Shop.Persistence.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly AppDBContext _context;
        private readonly ILogger<CategoryService> _logger;

        public PaymentMethodService(AppDBContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
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

        public IResult AddPaymentMethod(AddPaymentMethodDTO addPaymentMethodDTO, string locale)
        {
            if (string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorResult(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            AddPaymentMethodDTOValidation validationRules = new AddPaymentMethodDTOValidation(locale, SupportedLaunguages);
            var validationResult = validationRules.Validate(addPaymentMethodDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(locale,messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            try
            {

                PaymentMethod paymentMethod = new PaymentMethod
                {
                    IsCash = addPaymentMethodDTO.IsCash,

                };
                _context.PaymentMethods.Add(paymentMethod);
                foreach (var methodLang in addPaymentMethodDTO.Content)
                {
                    PaymentMethodLanguages paymentMethodLanguage = new()
                    {
                        LangCode = methodLang.Key,
                        Content = methodLang.Value,
                        PaymentMethodId = paymentMethod.Id
                    };
                    _context.PaymentMethodLanguages.Add(paymentMethodLanguage);
                }
                _context.SaveChanges();
                return new SuccessResult(locale,message: HttpStatusErrorMessages.Created[locale], HttpStatusCode.Created);

            }
            catch (Exception ex)
            {


                _logger.LogError(ex, ex.Message);
                return new ErrorResult(locale,message: ex.Message, HttpStatusCode.BadRequest);
            }

        }

        public IResult DeletePaymentMethod(Guid id, string locale)
        {
            if (id == default || string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorResult(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            try
            {
                PaymentMethod? paymentMethod = _context.PaymentMethods.FirstOrDefault(x => x.Id == id);
                if (paymentMethod is null)
                    return new ErrorResult(locale,message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
                _context.PaymentMethods.Remove(paymentMethod);
                _context.SaveChanges();
                return new SuccessResult(locale,message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {


                _logger.LogError(ex, ex.Message);
                return new ErrorResult(locale,message: ex.Message, HttpStatusCode.BadRequest);
            }
        }

        public IDataResult<IQueryable<GetPaymentMethodDTO>> GetAllPaymentMethods(string locale)
        {
            if (string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<IQueryable<GetPaymentMethodDTO>>(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            return new SuccessDataResult<IQueryable<GetPaymentMethodDTO>>(_context.PaymentMethods
                     .Select(x => new GetPaymentMethodDTO
                     {
                         Id = x.Id,
                         IsCash = x.IsCash,
                         Content = x.PaymentMethodLanguages.FirstOrDefault(y => y.LangCode == locale).Content

                     }).AsNoTracking(), locale,message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);

        }

        public async Task<IDataResult<PaginatedList<GetPaymentMethodDTO>>> GetAllPaymentMethodsByPageOrSearchAsync(int page, string locale,string? search=null)
        {
            if (string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<PaginatedList<GetPaymentMethodDTO>>(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            if (page < 1)
                page = 1;
            var query = search is null ? _context.PaymentMethods.AsNoTracking().AsSplitQuery().Select(x => new GetPaymentMethodDTO
            {
                Id = x.Id,
                IsCash = x.IsCash,
                Content = x.PaymentMethodLanguages.FirstOrDefault(y => y.LangCode == locale).Content
            }) : _context.PaymentMethods.AsNoTracking().AsSplitQuery().Select(x => new GetPaymentMethodDTO
            {
                Id = x.Id,
                IsCash = x.IsCash,
                Content = x.PaymentMethodLanguages.FirstOrDefault(y => y.LangCode == locale).Content
            }).Where(x=>x.Content.ToLower().Contains(search.ToLower()));

            var paginatedList = await PaginatedList<GetPaymentMethodDTO>.CreateAsync(query, page, 10);
            return new SuccessDataResult<PaginatedList<GetPaymentMethodDTO>>(paginatedList, locale,message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public async Task<IDataResult<GetPaymentMethodDetailDTO>> GetPaymentMethodByIdAsync(Guid id, string locale)
        {
            if (id == default || string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<GetPaymentMethodDetailDTO>(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);

            GetPaymentMethodDetailDTO? getPaymentMethodDTO = await _context.PaymentMethods.AsNoTracking().Where(x => x.Id == id).Select(x => new GetPaymentMethodDetailDTO
            {
                Id = x.Id,
                IsCash = x.IsCash,
                Content = x.PaymentMethodLanguages.Select(y => new KeyValuePair<string, string>(y.LangCode, y.Content)).ToDictionary()
            }).FirstOrDefaultAsync();
            if (getPaymentMethodDTO is null)
                return new ErrorDataResult<GetPaymentMethodDetailDTO>(locale,message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);

            return new SuccessDataResult<GetPaymentMethodDetailDTO>(getPaymentMethodDTO, locale,message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public IResult UdpatePaymentMethod(UpdatePaymentMethodDTO updatePaymentMethodDTO, string locale)
        {

            if (string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorResult(DefaultLaunguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLaunguage], HttpStatusCode.UnsupportedMediaType);
            UpdatePaymentMethodDTOValidation validationRules = new UpdatePaymentMethodDTOValidation(locale, SupportedLaunguages);
            var validationResult = validationRules.Validate(updatePaymentMethodDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(locale,messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            try
            {

                PaymentMethod? paymentMethod = _context.PaymentMethods.Include(x => x.PaymentMethodLanguages).FirstOrDefault(x => x.Id == updatePaymentMethodDTO.Id);
                if (paymentMethod is null)
                    return new ErrorResult(locale,message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
                paymentMethod.IsCash = updatePaymentMethodDTO.IsCash;
                foreach (var methodLang in updatePaymentMethodDTO.Content)
                {
                    PaymentMethodLanguages? Language = paymentMethod.PaymentMethodLanguages.FirstOrDefault(x => x.LangCode == methodLang.Key);
                    if (Language is not null)
                    {
                        Language.Content = methodLang.Value;
                        _context.PaymentMethodLanguages.Update(Language);
                    }
                    else
                    {
                        PaymentMethodLanguages newLang = new PaymentMethodLanguages
                        {
                            PaymentMethodId = paymentMethod.Id,
                            LangCode = methodLang.Key,
                            Content = methodLang.Value
                        };
                        _context.PaymentMethodLanguages.Add(newLang);
                    }
                }
                _context.PaymentMethods.Update(paymentMethod);
                _context.SaveChanges();
                return new SuccessResult(locale,message: HttpStatusErrorMessages.Created[locale], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(locale,message: ex.Message, HttpStatusCode.BadRequest);
            }
    }
        
    } }
