namespace Shop.Application.DTOs.ProductDTOs
{
   public class GetSizeForProductDTO
    {
        public Guid Id { get; set; }
        public string Size { get; set; }
        public int StockCount { get; set; }

    }
}
