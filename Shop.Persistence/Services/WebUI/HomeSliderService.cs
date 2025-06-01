using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI.HomeSliderItemDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Persistence.Context;

namespace Shop.Persistence.Services.WebUI
{
    public class HomeSliderService : IHomeSliderService
    {
        private readonly ILogger<HomeSliderService> _logger;
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
        public Task<IResult> AddHomeSliderItemAsync(AddHomeSliderItemDTO addHomeSliderItemDTO, string LangCode)
        {
            throw new NotImplementedException();
        }

        public IResult DeleteHomeSliderItem(Guid Id, string LangCode)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<PaginatedList<GetHomeSliderItemDTO>>> GetAllHomeSliderAsync(string LangCode, int page)
        {
            throw new NotImplementedException();
        }

        public IDataResult<IQueryable<GetHomeSliderItemForUIDTO>> GetHomeSliderItemForUI(string LangCode)
        {
            throw new NotImplementedException();
        }

        public IDataResult<GetHomeSliderItemForUpdateDTO> GetHomeSliderItemForUpdate(Guid Id, string LangCode)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateHomeSliderItemAsync(UpdateHomeSliderItemDTO updateHomeSliderItemDTO, string LangCode)
        {
            throw new NotImplementedException();
        }
    }
}
