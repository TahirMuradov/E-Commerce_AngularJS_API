using Shop.Domain.Entities.Common;

namespace Shop.Domain.Entities.WebUIEntites
{
    public class DisCountArea:BaseEntity
    {
        public bool IsActive { get; set; }
        public ICollection<DisCountAreaLanguage> Languages { get; set; }
    }
}
