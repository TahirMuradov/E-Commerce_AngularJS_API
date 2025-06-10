using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs
{
   public class AddTopCategoryAreaDTO
    {
        [ModelBinder(BinderType = typeof(ModelBindingDictionary<string, string>))]
        public Dictionary<string, string> Title { get; set; }
        [ModelBinder(BinderType = typeof(ModelBindingDictionary<string, string>))]

        public Dictionary<string, string> Description { get; set; }
        public Guid? CategoryId { get; set; }
        public IFormFile Image { get; set; }
    }
}
