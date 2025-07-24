namespace Shop.Application.DTOs.PaymentMethodDTOs
{
   public class AddPaymentMethodDTO
    {
        public bool IsCash { get; set; }
        /// <summary>
        /// The name of the category to be added.
        /// key is the culture code (e.g., "en", "ru","az").
        /// value is the localized PaymentMethod of the content.
        /// </summary>
        public Dictionary<string,string> Content { get; set; }
    }
}
