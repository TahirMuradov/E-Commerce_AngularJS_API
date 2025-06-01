namespace Shop.Application.DTOs.WebUI.DisCountAreaDTOs
{
   public class GetDisCountAreaForUpdateDTO
    {
        public Guid Id { get; set; }
        public Dictionary<string, string> TitleContent { get; set; }
        public Dictionary<string, string> DescriptionContent { get; set; }
    }
}
