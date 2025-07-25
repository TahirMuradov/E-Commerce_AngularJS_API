﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.SizeDTOs;
using Shop.Domain.Exceptions;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Application.Validators.SizeValidations;
using Shop.Domain.Entities;
using Shop.Persistence.Context;
using System.Net;

namespace Shop.Persistence.Services
{
    public class SizeService : ISizeService
    {
        private readonly AppDBContext _context;
        private readonly ILogger<SizeService> _logger;
        private string[] SupportedLaunguages
        {
            get
            {



                return ConfigurationPersistence.SupportedLaunguageKeys;


            }
        }
        private string DefaultLanguage
        {
            get
            {
                return ConfigurationPersistence.DefaultLanguageKey;
            }
        }


        public SizeService(AppDBContext context, ILogger<SizeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IResult AddSize(AddSizeDTO addSizeDTO, string locale)
        {
            if (string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorResult(DefaultLanguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLanguage], HttpStatusCode.UnsupportedMediaType);


            AddSizeDTOValidation validationRules = new AddSizeDTOValidation();
            var validationResult = validationRules.Validate(addSizeDTO);
            if (!validationResult.IsValid)
                return new SuccessResult(locale,validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);


            try
            {
                Size size = new Size()
                {
                    Content=addSizeDTO.Size,

                };
                _context.Sizes.Add(size);
                _context.SaveChanges();
                return new SuccessResult(locale,message: HttpStatusErrorMessages.Created[locale], HttpStatusCode.Created);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return new ErrorResult(locale,message: ex.Message, HttpStatusCode.BadRequest);
            }

        }

        public IResult DeleteSize(Guid id, string locale)
        {
            if (id==default||string.IsNullOrEmpty(locale)||!SupportedLaunguages.Contains(locale))
                return new ErrorResult(locale,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLanguage], HttpStatusCode.UnsupportedMediaType);
            Size? size = _context.Sizes.FirstOrDefault(x => x.Id == id);
            if (size == null)
                return new ErrorResult(locale,message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
            try {
            
            _context.Sizes.Remove(size);
                _context.SaveChanges();
                return new SuccessResult(locale,message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(locale,message: ex.Message, HttpStatusCode.BadRequest);
            }


        }

        public IDataResult<IQueryable<GetSizeDTO>> GetAllSizes(string locale)
        {
            if (string.IsNullOrEmpty(locale)|| !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<IQueryable<GetSizeDTO>>(locale,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLanguage], HttpStatusCode.UnsupportedMediaType);
            IQueryable<GetSizeDTO> querySize=_context.Sizes
                .Select(size => new GetSizeDTO
                {
                    Id = size.Id,
                    Size = size.Content
                }).AsNoTracking();
            return new SuccessDataResult<IQueryable<GetSizeDTO>>(querySize, locale,message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
        }

        public async Task<IDataResult<PaginatedList<GetSizeDTO>>> GetAllSizesByPageOrSearchAsync(int page, string locale,string? search=null)
        {
            if (string.IsNullOrEmpty(locale)||!SupportedLaunguages.Contains(locale))
        return new ErrorDataResult<PaginatedList<GetSizeDTO>>(locale,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLanguage], HttpStatusCode.UnsupportedMediaType);
            if (page < 1)
                page = 1;
            IQueryable<GetSizeDTO> querySize = search is null? _context.Sizes.Select(size => new GetSizeDTO
            {
                Id = size.Id,
                Size = size.Content
            }).AsNoTracking().AsSingleQuery():
            _context.Sizes.Select(size => new GetSizeDTO
            {
                Id = size.Id,
                Size = size.Content
            }).AsNoTracking().AsSingleQuery().Where(x=>x.Size.ToLower().Contains(search.ToLower())||x.Id.ToString().ToLower().Contains(search.ToLower()));


            PaginatedList<GetSizeDTO> paginatedList = await PaginatedList<GetSizeDTO>.CreateAsync(querySize, page, 10);
            return new SuccessDataResult<PaginatedList<GetSizeDTO>>(paginatedList, locale,message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);


        }

        public IDataResult<GetSizeDTO> GetSizeById(Guid id, string locale)
        {

            if (id==default|| string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<GetSizeDTO>(DefaultLanguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLanguage], HttpStatusCode.UnsupportedMediaType);
            GetSizeDTO? getSizeDTO = _context.Sizes
                .Where(size => size.Id == id)
                .Select(size => new GetSizeDTO
                {
                    Id = size.Id,
                    Size = size.Content
                }).AsNoTracking().FirstOrDefault();
            if (getSizeDTO == null)
                return new ErrorDataResult<GetSizeDTO>(locale,message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
            return new SuccessDataResult<GetSizeDTO>(getSizeDTO, locale,message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);

        }

        public IResult UpdateSize(UpdateSizeDTO updateSizeDTO, string locale)
        {
            if ( string.IsNullOrEmpty(locale) || !SupportedLaunguages.Contains(locale))
                return new ErrorDataResult<GetSizeDTO>(DefaultLanguage,message: HttpStatusErrorMessages.UnsupportedLanguage[DefaultLanguage], HttpStatusCode.UnsupportedMediaType);
            UpdateSizeDTOValidation validationRules = new UpdateSizeDTOValidation();
            var validationResult = validationRules.Validate(updateSizeDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(locale,messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
           
            try
            {
            Size? size = _context.Sizes.FirstOrDefault(x => x.Id == updateSizeDTO.Id);
            if (size is null)
                return new ErrorResult(locale,message: HttpStatusErrorMessages.NotFound[locale], HttpStatusCode.NotFound);
                size.Content = updateSizeDTO.Size;
                _context.Sizes.Update(size);
                _context.SaveChanges();
                return new SuccessResult(locale,message: HttpStatusErrorMessages.Success[locale], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ErrorResult(locale,message: ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}
