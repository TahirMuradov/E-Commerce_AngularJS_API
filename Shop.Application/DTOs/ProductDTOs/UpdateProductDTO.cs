using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Application.DTOs.ProductDTOs
{
   public class UpdateProductDTO
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        [ModelBinder(BinderType = typeof(ModelBindingDictionary<string, string>))]
        /// <summary>
        /// The name of the category to be added.
        /// key is the culture code (e.g., "en", "ru","az").
        /// value is the localized Product of the title.
        /// </summary>
        public Dictionary<string, string> Title { get; set; }

        [ModelBinder(BinderType = typeof(ModelBindingDictionary<string, string>))]
        /// <summary>
        /// The name of the category to be added.
        /// key is the culture code (e.g., "en", "ru","az").
        /// value is the localized Product of the description.
        /// </summary>
        public Dictionary<string, string> Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public Guid CategoryId { get; set; }
        public bool Isfeature { get; set; }

        [ModelBinder(BinderType = typeof(ModelBindingDictionary<Guid, int>))]
        /// <summary>
        /// The name of the category to be added.
        /// key is the Id of Size .
        /// value is the product`s Size of stock count.
        /// </summary>
        public Dictionary<Guid, int> Sizes { get; set; }
        public IFormFileCollection? NewImages { get; set; }
        public ICollection<string>? DeletedImageUrls { get; set; }

    }
}
