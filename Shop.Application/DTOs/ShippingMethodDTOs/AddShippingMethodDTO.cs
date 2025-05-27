namespace Shop.Application.DTOs.ShippingMethodDTOs
{
   public class AddShippingMethodDTO
    {
        /// <summary>
        /// The name of the category to be added.
        /// key is the culture code (e.g., "en", "ru","az").
        /// value is the localized ShippingMethod of the content.
        /// </summary>
        public Dictionary<string,string> Content { get; set; }
        public decimal Price { get; set; }
        public decimal DisCountPrice { get; set; }
    }
}
