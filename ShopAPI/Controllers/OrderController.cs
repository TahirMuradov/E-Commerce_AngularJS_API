using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using Shop.Domain.Enums;
using Shop.Persistence;
using System.Security.Claims;

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
        public async Task<IActionResult> GetOrderById([FromQuery] Guid orderId)
        {
            string langCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()?.ToLower() ?? DefaultLaunguage;
            var result = await _orderService.GetOrderByIdAsync(orderId, langCode);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderByPage([FromQuery] int page, [FromQuery] string? search = null)
        {
            string langCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()?.ToLower() ?? DefaultLaunguage;
            var result = await _orderService.GetAllOrdersByPageOrSearchAsync(page, langCode,search);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateOrderStatus([FromQuery] Guid orderId, [FromBody] OrderStatus status)
        {
            string langCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()?.ToLower() ?? DefaultLaunguage;
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status, langCode);
            return StatusCode((int)result.StatusCode, result);



        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderDTO updateOrderDTO)
        {
            string langCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()?.ToLower() ?? DefaultLaunguage;
            var result = await _orderService.UpdateOrderAsync(updateOrderDTO, langCode);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteOrder([FromQuery] Guid orderId)
        {
            string langCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()?.ToLower() ?? DefaultLaunguage;
            var result = await _orderService.DeleteOrderAsync(orderId, langCode);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllOrdersByUserId([FromQuery] int page)
        {
            string langCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()?.ToLower() ?? DefaultLaunguage;
            string userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _orderService.GetAllOrdersByUserIdAsync(userId, page, langCode);
            return StatusCode((int)result.StatusCode, result);

        }
    }
}
