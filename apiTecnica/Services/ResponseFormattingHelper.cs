
using apiTecnica.Models;

namespace apiTecnica.Services
{
    public static class ResponseFormattingHelper
    {
        public static List<object> GenerateLines(List<SaleItem> saleItems)
        {
            List<object> lines = [];
            foreach (SaleItem item in saleItems)
            {
                lines.Add(new
                {
                    product_id = item.ProductId,
                    quantity = item.Quantity,
                    list_price = item.ListPriceAtSale,
                    discounts = new
                    {
                        product_type = item.DiscountProductTypeAmount,
                        payment_method = item.DiscountPaymentMethodAmount,
                        credit_terms = item.DiscountCreditTermsAmount
                    },
                    line_subtotal_after_discounts = item.LineSubtotalAfterDiscounts
                });
            }
            return lines;
        }
    }
}