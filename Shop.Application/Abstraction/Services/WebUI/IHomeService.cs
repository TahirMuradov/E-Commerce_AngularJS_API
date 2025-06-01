using Shop.Application.DTOs.WebUI;
using Shop.Application.ResultTypes.Abstract;

namespace Shop.Application.Abstraction.Services.WebUI
{
  public  interface IHomeService
    {
      public  IDataResult<GetHomeAllDataDTO> GetHomeAllData(string LangCode);
    }
}
