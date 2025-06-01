namespace Shop.Application.DTOs.WebUI.DisCountAreaDTOs
{
   public class UpdateDisCountAreaDTO
    {
        public Guid Id { get; set; }
        public Dictionary<string, string> TitleContent { get; set; }
        public Dictionary<string, string> DescriptionContent { get; set; }
    }
}
