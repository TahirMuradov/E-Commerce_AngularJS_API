namespace Shop.Application.DTOs.CategoryDTOs
{
    public class GetCategoryDTO
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsFEatured { get; set; }

    }
}
