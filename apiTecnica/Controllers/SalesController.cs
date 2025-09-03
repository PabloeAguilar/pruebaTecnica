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
            public long product_id { get; set; }
            public decimal quantity { get; set; }
        }

        public class SaleRequestDTO
        {
            public long customer_id { get; set; }
            public string payment_method { get; set; } = string.Empty;
            public List<SaleItemDTO> items { get; set; } = new();
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
            if (request.items == null || !request.items.Any())
                return BadRequest("Items are required.");

            using (var context = new PruebaTecnicaContext())
            {
                var customer = await context.Customers.FindAsync(request.customer_id);
                if (customer == null)
                    return NotFound($"Customer with ID {request.customer_id} not found.");

                var paymentMethod = await context.PaymentMethods.Include(p => p.DiscountByPaymentMethod).FirstOrDefaultAsync(pm => pm.DisplayName == request.payment_method);
                if (paymentMethod == null)
                    return NotFound($"Payment method '{request.payment_method}' not found.");

                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var taxRate = await context.TaxRates
                    .FirstOrDefaultAsync(tr =>
                        tr.DeletedAt == null &&
                        tr.EffectiveFrom <= today &&
                        (tr.EffectiveTo == null || tr.EffectiveTo > today)
                    );

                foreach (var item in request.items)
                {
                    if (item.quantity <= 0)
                    {
                        return BadRequest($"Invalid quantity");
                    }
                    var product = await context.Products.FindAsync(item.product_id);
                    if (product == null)
                        return NotFound($"Product with ID {item.product_id} not found.");
                }

                var sale = new Sale
                {
                    CustomerId = request.customer_id,
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    SaleDatetime = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedIp = HttpContext.Connection.RemoteIpAddress?.ToString()
                };
                context.Sales.Add(sale);
                await context.SaveChangesAsync();

                List<SaleItem> currentItems = [];
                foreach (var item in request.items)
                {
                    var product = await context.Products.Include(p => p.ProductType).FirstOrDefaultAsync(p => p.ProductId == item.product_id);
                    var saleItem = new SaleItem
                    {
                        SaleId = sale.SaleId,
                        ProductId = item.product_id,
                        Quantity = item.quantity,
                        ListPriceAtSale = product.ListPrice,
                        CreatedAt = DateTime.UtcNow,
                        CreatedIp = HttpContext.Connection.RemoteIpAddress?.ToString()
                    };
                    PricingItemSaleHelper.ApplyDiscounts(saleItem, product.ProductType.DiscountByProductType, paymentMethod.DiscountByPaymentMethod, customer.CreditTerms);
                    currentItems.Add(saleItem);
                    context.SaleItems.Add(saleItem);
                }
                await context.SaveChangesAsync();
                var taxRatePercent = taxRate?.RatePercent ?? 0;

                var subtotal = currentItems.Sum(item => item.LineSubtotalAfterDiscounts);
                var tax = Math.Round(subtotal * taxRatePercent / 100, 2);
                var total = subtotal + tax;
                var totalDiscountsAmount = PricingItemSaleHelper.DiscountSum(currentItems);

                // Actualizar la entidad Sale con los datos calculados
                sale.SubtotalAmount = subtotal;
                sale.TaxAmount = tax;
                sale.TaxRatePercent = taxRatePercent;
                sale.TotalAmount = total;
                sale.TotalDiscountsAmount = totalDiscountsAmount;
                await context.SaveChangesAsync();

                var response = new
                {
                    sale_id = (int)sale.SaleId,
                    customer_id = (int)sale.CustomerId,
                    payment_method = request.payment_method,
                    tax_rate_percent = taxRate?.RatePercent ?? 0,
                    breakdown = new
                    {
                        lines = ResponseFormattingHelper.GenerateLines(currentItems)
                    },
                    subtotal,
                    tax,
                    total,
                    total_discounts_amount = totalDiscountsAmount
                };

                return CreatedAtAction(nameof(PostSale), new { id = sale.SaleId }, response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSales()
        {
            using (var context = new PruebaTecnicaContext())
            {
                var sales = await context.Sales
                    .Where(s => s.DeletedAt == null)
                    .Include(s => s.PaymentMethod)
                    .Select(s => new
                    {
                        sale_id = s.SaleId,
                        customer_id = s.CustomerId,
                        payment_method = s.PaymentMethod.DisplayName,
                        subtotal = s.SubtotalAmount,
                        tax = s.TaxAmount,
                        total = s.TotalAmount,
                        total_discounts_amount = s.TotalDiscountsAmount,
                        sale_datetime = s.SaleDatetime
                    }).OrderBy(s => s.sale_id)
                    .ToListAsync();
                return Ok(sales);
            }
        }

    }





    public class Line
    {
        public int product_id { get; set; }
        public int quantity { get; set; }
        public int list_price { get; set; }


    }
}
