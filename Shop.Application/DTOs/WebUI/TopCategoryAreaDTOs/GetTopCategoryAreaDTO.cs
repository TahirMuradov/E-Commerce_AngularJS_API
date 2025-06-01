namespace Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs
{
   public class GetTopCategoryAreaDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? CategoryId { get; set; }
        public string PictureUrl { get; set; }
    }
}
