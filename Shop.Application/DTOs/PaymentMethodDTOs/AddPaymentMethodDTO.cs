namespace Shop.Application.DTOs.PaymentMethodDTOs
{
   public class AddPaymentMethodDTO
    {
        public bool IsCash { get; set; }
        public Dictionary<string,string> Content { get; set; }
    }
}
