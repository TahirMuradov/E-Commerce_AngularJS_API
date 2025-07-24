using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Application.DTOs.WebUI.HomeSliderItemDTOs
{
  public  class UpdateHomeSliderItemDTO
    {
        public Guid Id { get; set; }
        [ModelBinder(BinderType = typeof(ModelBindingDictionary<string, string>))]
        public Dictionary<string, string> Title { get; set; }
        [ModelBinder(BinderType = typeof(ModelBindingDictionary<string, string>))]

        public Dictionary<string, string> Description { get; set; }
        public IFormFile? NewImage { get; set; }


    }
}
