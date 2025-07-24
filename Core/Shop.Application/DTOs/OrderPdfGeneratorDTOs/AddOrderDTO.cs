namespace Shop.Application.DTOs.OrderPdfGeneratorDTOs
{
    public class AddOrderDTO
    {
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public List<OrderProductDTO> Products { get; set; }
        public OrderShippingMethodDTO  ShippingMethod { get; set; }
        public OrderPaymentMethodDTO PaymentMethod { get; set; }



    }
}
