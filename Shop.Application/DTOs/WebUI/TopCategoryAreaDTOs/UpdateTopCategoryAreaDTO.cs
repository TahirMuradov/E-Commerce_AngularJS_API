using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs
{
    public class UpdateTopCategoryAreaDTO
    {
        public Guid Id { get; set; }
        [ModelBinder(BinderType = typeof(ModelBindingDictionary<string, string>))]

        public Dictionary<string, string> Title { get; set; }
        [ModelBinder(BinderType = typeof(ModelBindingDictionary<string, string>))]

        public Dictionary<string, string> Description { get; set; }
        public bool IsActive { get; set; }
        public Guid? SubCategoryId { get; set; }
        public Guid? CategoryId { get; set; }
       
        public string? CurrentPictureUrl { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
