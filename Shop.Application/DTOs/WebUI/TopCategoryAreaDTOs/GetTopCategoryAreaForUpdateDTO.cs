namespace Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs
{
    public class GetTopCategoryAreaForUpdateDTO
    {
        public Guid Id { get; set; }
        public Dictionary<string, string> Title { get; set; }
        public Dictionary<string, string> Description { get; set; }
        public string? CategoryId { get; set; }
        public string PictureUrl { get; set; }
    }
}
