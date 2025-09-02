using Microsoft.AspNetCore.Mvc;
using apiTecnica.Models;
using Microsoft.EntityFrameworkCore;
using apiTecnica.Services;
using System.Runtime.InteropServices;

namespace apiTecnica.Controllers
{
    [ApiController]
    [Route("sales")]
    public class SalesController : ControllerBase
    {
        public class SaleItemDTO
        {
            public long ProductId { get; set; }
            public decimal Quantity { get; set; }
        }

        public class SaleRequestDTO
        {
            public long CustomerId { get; set; }
            public string PaymentMethod { get; set; } = string.Empty;
            public List<SaleItemDTO> Items { get; set; } = new();
        }

        public class SaleResponseDTO
        {
            public int SaleId { get; set; }
            public int CustomerId { get; set; }
            public string? PaymentMethod { get; set; }
            public List<SaleItemDTO>? Items { get; set; }
            public decimal TaxRatePercent { get; set; }


            public decimal Subtotal { get; set; }
            public decimal Tax { get; set; }

            public decimal Total { get; set; }

            public decimal TotalDiscountsAmount { get; set; }

        }

        [HttpPost]
        public async Task<IActionResult> PostSale([FromBody] SaleRequestDTO request)
        {
            if (request.Items == null || !request.Items.Any())
                return BadRequest("Items are required.");

            using (var context = new PruebaTecnicaContext())
            {
                Console.WriteLine("CUSTOMER" + request.CustomerId);
                var customer = await context.Customers.FindAsync(request.CustomerId);
                if (customer == null)
                    return NotFound($"Customer with ID {request.CustomerId} not found.");

                var paymentMethod = await context.PaymentMethods.FirstOrDefaultAsync(pm => pm.DisplayName == request.PaymentMethod);
                if (paymentMethod == null)
                    return NotFound($"Payment method '{request.PaymentMethod}' not found.");

                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var taxRate = await context.TaxRates
                    .FirstOrDefaultAsync(tr =>
                        tr.DeletedAt == null &&
                        tr.EffectiveFrom <= today &&
                        (tr.EffectiveTo == null || tr.EffectiveTo > today)
                    );

                foreach (var item in request.Items)
                {
                    if (item.Quantity <= 0)
                    {
                        return BadRequest($"Invalid quantity on Product with ID {item.ProductId}");
                    }
                    var product = await context.Products.FindAsync(item.ProductId);
                    if (product == null)
                        return NotFound($"Product with ID {item.ProductId} not found.");
                }


                var sale = new Sale
                {
                    CustomerId = request.CustomerId,
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    SaleDatetime = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedIp = HttpContext.Connection.RemoteIpAddress?.ToString()
                };
                context.Sales.Add(sale);
                await context.SaveChangesAsync();

                List<SaleItem> currentItems = [];
                foreach (var item in request.Items)
                {
                    // todo: Mover validación antes de iniciar procesado de venta
                    var product = await context.Products.FindAsync(item.ProductId);
                    var saleItem = new SaleItem
                    {
                        SaleId = sale.SaleId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        ListPriceAtSale = product.ListPrice,
                        CreatedAt = DateTime.UtcNow,
                        CreatedIp = HttpContext.Connection.RemoteIpAddress?.ToString()
                    };
                    PricingItemSaleHelper.ApplyDiscounts(saleItem, product.ProductType.DiscountByProductType, paymentMethod.DiscountByPaymentMethod, customer.CreditTerms);
                    currentItems.Add(saleItem);
                    context.SaleItems.Add(saleItem);
                }
                await context.SaveChangesAsync();

                var subtotal = currentItems.Sum(item => item.LineSubtotalAfterDiscounts);
                var tax = Math.Round(subtotal * taxRate.RatePercent, 2);
                var response = new
                {
                    sale_id = (int)sale.SaleId,
                    customer_id = (int)sale.CustomerId,
                    payment_method = request.PaymentMethod,
                    tax_rate_percent = taxRate?.RatePercent ?? 0,
                    breakdown = new
                    {
                        lines = ResponseFormattingHelper.GenerateLines(currentItems)
                    },
                    subtotal,
                    tax,
                    total = subtotal + tax,
                    total_discounts_amount = PricingItemSaleHelper.DiscountSum(currentItems)
                };
                return CreatedAtAction(nameof(PostSale), new { id = sale.SaleId }, response);
            }
        }
    }

    public class Line {
        public int product_id { get; set; }
        public int quantity { get; set; }
        public int list_price { get; set; }
        

    }
}
