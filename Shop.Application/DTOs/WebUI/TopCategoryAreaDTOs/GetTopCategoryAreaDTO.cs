namespace Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs
{
   public class GetTopCategoryAreaDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? CategoryName{ get; set; }
        public string PictureUrl { get; set; }
    }
}
