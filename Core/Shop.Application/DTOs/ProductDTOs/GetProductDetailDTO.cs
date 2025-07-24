namespace Shop.Application.DTOs.ProductDTOs
{
    public class GetProductDetailDTO
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public List<string> ImageUrls { get; set; }
        public ICollection<GetSizeForProductDetailDTO> Sizes { get; set; }
        public ICollection<GetProductDTO> RelatedProducts { get; set; }
    }
}
