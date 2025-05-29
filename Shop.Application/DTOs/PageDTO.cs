namespace Shop.Application.DTOs
{
   public class PageDTO
    {
        public readonly int MaxProductCount = 9;
        public int PageSize { get; set; }
        public int ActivePage { get; set; } = 1;
    }
}
