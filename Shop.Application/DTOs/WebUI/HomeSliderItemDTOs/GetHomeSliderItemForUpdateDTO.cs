namespace Shop.Application.DTOs.WebUI.HomeSliderItemDTOs
{
  public  class GetHomeSliderItemForUpdateDTO
    {
        public Guid Id { get; set; }
        public Dictionary<string, string> Title { get; set; }
        public Dictionary<string, string> Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
