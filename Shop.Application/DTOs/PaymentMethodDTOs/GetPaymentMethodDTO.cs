namespace Shop.Application.DTOs.PaymentMethodDTOs
{
   public class GetPaymentMethodDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool IsCash { get; set; }
    }
}
