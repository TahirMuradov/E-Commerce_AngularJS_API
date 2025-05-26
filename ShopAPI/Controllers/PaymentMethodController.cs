using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.PaymentMethodDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethod;

        public PaymentMethodController(IPaymentMethodService paymentMethod)
        {
            _paymentMethod = paymentMethod;
        }
        [HttpPost("[action]")]
        public IActionResult AddPaymentMethod([FromBody]AddPaymentMethodDTO addPaymentMethodDTO)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _paymentMethod.AddPaymentMethod(addPaymentMethodDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut("[action]")]
        public IActionResult UdpatePaymentMethod([FromBody]UpdatePaymentMethodDTO updatePaymentMethodDTO)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _paymentMethod.UdpatePaymentMethod(updatePaymentMethodDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("[action]")]
        public IActionResult DeletePaymentMethod([FromQuery]Guid id)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _paymentMethod.DeletePaymentMethod(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpDelete("[action]")]
        public IActionResult GetPaymentMethodById([FromQuery]Guid id)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _paymentMethod.GetPaymentMethodById(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllPaymentMethods(){
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _paymentMethod.GetAllPaymentMethods(headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPaymentMethodsByPage([FromQuery]int page)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = await _paymentMethod.GetAllPaymentMethodsByPageAsync(page, headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }

    }
}
