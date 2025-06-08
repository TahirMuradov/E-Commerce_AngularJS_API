namespace Shop.Application.DTOs.ProductDTOs
{
   public class GetProductForUpdateDTO
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        //key is the lang code (e.g., "en", "ru","az") and value is the localized Product of the title.
        public Dictionary<string,string> Title { get; set; }
        //key is the lang code (e.g., "en", "ru","az") and value is the localized Product of the description.

        public Dictionary<string,string> Description { get; set; }
       
        public Guid  CategoryId { get; set; }
        public bool IsFeature { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public List<string> ImageUrls { get; set; }
        public ICollection<GetSizeForProductDetailDTO> Sizes { get; set; }
      
    }
}
