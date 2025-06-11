using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.PaymentMethodDTOs;
using Shop.Persistence;
using System.Threading.Tasks;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class PaymentMethodController : ControllerBase
    {
        private string DefaultLaunguage
        {
            get
            {
                return Configuration.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        private readonly IPaymentMethodService _paymentMethod;
        private readonly IHttpContextAccessor _contextAccessor;
        public PaymentMethodController(IPaymentMethodService paymentMethod, IHttpContextAccessor contextAccessor)
        {
            _paymentMethod = paymentMethod;
            _contextAccessor = contextAccessor;
        }
        [HttpPost("[action]")]
        public IActionResult AddPaymentMethod([FromBody] AddPaymentMethodDTO addPaymentMethodDTO)
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _paymentMethod.AddPaymentMethod(addPaymentMethodDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut("[action]")]
        public IActionResult UdpatePaymentMethod([FromBody] UpdatePaymentMethodDTO updatePaymentMethodDTO)
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _paymentMethod.UdpatePaymentMethod(updatePaymentMethodDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("[action]")]
        public IActionResult DeletePaymentMethod([FromQuery] Guid id)
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _paymentMethod.DeletePaymentMethod(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetPaymentMethodById([FromQuery] Guid id)
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = await _paymentMethod.GetPaymentMethodByIdAsync(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllPaymentMethods()
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = _paymentMethod.GetAllPaymentMethods(headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPaymentMethodsByPageOrSearch([FromQuery] int page, [FromQuery] string? search = null)
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = await _paymentMethod.GetAllPaymentMethodsByPageOrSearchAsync(page, headerLocale, search);
            return StatusCode((int)result.StatusCode, result);

        }

    }
}
