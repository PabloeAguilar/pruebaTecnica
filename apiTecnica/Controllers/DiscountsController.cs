using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiTecnica.Models;

namespace apiTecnica.Controllers
{
    [ApiController]
    [Route("/discounts")]
    public class DiscountsController : ControllerBase
    {
        public class ProductDiscountDTO
        {
            public string? ProductType { get; set; }
            public decimal DiscountPercent { get; set; }
        }

        [HttpPost("product")]
        public IActionResult PostProductDiscount([FromBody] ProductDiscountDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.ProductType))
                return BadRequest("ProductType is required.");

            if (request.DiscountPercent < 0)
                return BadRequest("Discount can't be less than 0");


            using (var context = new PruebaTecnicaContext())
            {

                var productType = context.ProductTypes.FirstOrDefault(pt => pt.Name == request.ProductType);
                if (productType == null)
                    return NotFound($"ProductType '{request.ProductType}' not found.");

                var existingDiscount = context.DiscountByProductTypes.FirstOrDefault(d => d.ProductTypeId == productType.ProductTypeId && d.DeletedAt == null);
                if (existingDiscount != null)
                    return Conflict($"Discount for product type '{request.ProductType}' already exists.");

                var discount = new DiscountByProductType
                {
                    ProductTypeId = productType.ProductTypeId,
                    DiscountPercent = request.DiscountPercent,
                    CreatedAt = DateTime.UtcNow,
                    CreatedIp = HttpContext.Connection.RemoteIpAddress?.ToString()
                };
                context.DiscountByProductTypes.Add(discount);
                context.SaveChanges();
                return CreatedAtAction(nameof(PostProductDiscount), new { id = discount.ProductTypeId }, new
                {
                    product_type = request.ProductType,
                    discount_percent = request.DiscountPercent
                });
            }
        }

        public class PaymentMethodDiscountDTO
        {
            public string? PaymentMethod { get; set; }
            public decimal DiscountPercent { get; set; }
        }

        [HttpPost("payment")]
        public IActionResult PostPaymentMethodDiscount([FromBody] PaymentMethodDiscountDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.PaymentMethod))
                return BadRequest("PaymentMethod is required.");
            if (request.DiscountPercent < 0)
                return BadRequest("Discount can't be less than 0");

            using (var context = new PruebaTecnicaContext())
            {
                var paymentMethod = context.PaymentMethods.FirstOrDefault(pm => pm.DisplayName == request.PaymentMethod);
                if (paymentMethod == null)
                    return NotFound($"PaymentMethod '{request.PaymentMethod}' not found.");

                var existingDiscount = context.DiscountByPaymentMethods.FirstOrDefault(pm => pm.PaymentMethodId == paymentMethod.PaymentMethodId && pm.DeletedAt == null);
                if (existingDiscount != null)
                    return Conflict($"Discount for payment method '{request.PaymentMethod}' already exists.");

                var discount = new DiscountByPaymentMethod
                {
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    DiscountPercent = request.DiscountPercent,
                    CreatedAt = DateTime.UtcNow,
                    CreatedIp = HttpContext.Connection.RemoteIpAddress?.ToString()
                };
                context.DiscountByPaymentMethods.Add(discount);
                context.SaveChanges();

                return CreatedAtAction(nameof(PostPaymentMethodDiscount), new { id = discount.PaymentMethodId }, new
                {
                    payment_method = request.PaymentMethod,
                    discount_percent = request.DiscountPercent
                });
            }
        }

    }
}