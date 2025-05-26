namespace Shop.Application.DTOs.PaymentMethodDTOs
{
   public class UpdatePaymentMethodDTO
    {
        public Guid Id { get; set; }
        public bool IsCash { get; set; }
        public Dictionary<string, string> Content { get; set; }
    }
}
