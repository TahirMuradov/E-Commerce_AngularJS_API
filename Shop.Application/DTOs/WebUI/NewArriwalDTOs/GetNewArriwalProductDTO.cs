﻿namespace Shop.Application.DTOs.WebUI.NewArriwalDTOs
{
public class GetNewArriwalProductDTO
    {
        public Guid Id { get; set; }
        public string ImgUrl { get; set; }
        public decimal Price { get; set; }
        public decimal DisCount { get; set; }
        public string Title { get; set; }
        public GetIsFeaturedCategoryDTO Category { get; set; }
    }
}
