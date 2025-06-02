namespace Shop.Application.DTOs.CategoryDTOs
{
  public  class AddCategoryDTO
    {
        /// <summary>
        /// The name of the category to be added.
        /// key is the culture code (e.g., "en", "ru","az").
        /// value is the localized name of the category.
        /// </summary>
       
        public Dictionary<string,string> CategoryContent { get; set; }
        public bool IsFeatured { get; set; }
    }
}
