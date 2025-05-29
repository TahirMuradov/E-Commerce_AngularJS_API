using Shop.Application.DTOs.AuthDTOs;
using Shop.Domain.Enums;

namespace Shop.Application.DTOs.OrderPdfGeneratorDTOs
{
  public  class GetOrderDTO
    {
        public Guid Id { get; set; }
        public string  OrderNumber { get; set; }
        public string OrderPdfPath { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus  Status { get; set; }

        public GetUserDTO OrderBy { get; set; }

    }
}
