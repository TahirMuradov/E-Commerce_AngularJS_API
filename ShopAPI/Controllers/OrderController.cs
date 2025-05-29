using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using Shop.Domain.Enums;
using Shop.Persistence;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class OrderController : ControllerBase
    {
        private string DefaultLaunguage
        {
            get
            {
                return Configuration.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderController(IOrderService orderService, IHttpContextAccessor contextAccessor)
        {
            _orderService = orderService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderDTO addOrderDTO)
        {
            string langCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()?.ToLower() ?? DefaultLaunguage;
            var result = await _orderService.AddOrderAsync(addOrderDTO, langCode);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderById([FromQuery]Guid orderId)
        {
            string langCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()?.ToLower() ?? DefaultLaunguage;
            var result = await _orderService.GetOrderByIdAsync(orderId, langCode);
            
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderByPage([FromQuery]int page)
        {
            string langCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()?.ToLower() ?? DefaultLaunguage;
            var result = await _orderService.GetAllOrdersByPageAsync(page, langCode);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateOrderStatus([FromQuery] Guid orderId, [FromBody] OrderStatus status)
        {
            string langCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()?.ToLower() ?? DefaultLaunguage;
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status, langCode);
            return StatusCode((int)result.StatusCode, result);




        }
    }
}
