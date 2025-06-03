using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities.WebUIEntites
{
   public class DisCountAreaLanguage:BaseEntity
    {
        public string LangCode { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    
        public Guid DisCountAreaId { get; set; }
        public DisCountArea DisCountArea { get; set; }
     
    }
}
