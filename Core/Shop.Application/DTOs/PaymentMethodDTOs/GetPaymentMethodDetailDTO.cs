namespace Shop.Application.DTOs.PaymentMethodDTOs
{
   public class GetPaymentMethodDetailDTO
    {
        public Guid Id { get; set; }
        public Dictionary<string,string> Content { get; set; }
        public bool IsCash { get; set; }
    }
}
