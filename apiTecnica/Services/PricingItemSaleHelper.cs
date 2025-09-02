
using apiTecnica.Models;
using System;

namespace apiTecnica.Services
{
	public static class PricingItemSaleHelper
	{
		public static SaleItem ApplyDiscounts(
			SaleItem saleItem,
			DiscountByProductType discountProductType,
			DiscountByPaymentMethod discountPaymentMethod,
			CreditTerm discountCreditTerm)
		{
			var totalPreDiscount = Math.Round(saleItem.ListPriceAtSale * saleItem.ListPriceAtSale, 2);

			// Calcular descuentos individuales
			var discountProductTypeAmount = Math.Round(totalPreDiscount * (discountProductType?.DiscountPercent ?? 0) / 100m, 2);
			var discountPaymentMethodAmount = Math.Round(totalPreDiscount * (discountPaymentMethod?.DiscountPercent ?? 0) / 100m, 2);
			var discountCreditTermsAmount = Math.Round(totalPreDiscount * (discountCreditTerm?.DiscountPercent ?? 0) / 100m, 2);

			// Sumar descuentos
			var totalDiscount = discountProductTypeAmount + discountPaymentMethodAmount + discountCreditTermsAmount;

			// Calcular subtotal después de descuentos
			var lineSubtotalAfterDiscounts = totalPreDiscount - totalDiscount;

			// Actualizar SaleItem
			saleItem.DiscountProductTypeAmount = discountProductTypeAmount;
			saleItem.DiscountPaymentMethodAmount = discountPaymentMethodAmount;
			saleItem.DiscountCreditTermsAmount = discountCreditTermsAmount;
			saleItem.LineSubtotalAfterDiscounts = lineSubtotalAfterDiscounts;

			return saleItem;
		}

		public static decimal DiscountSum(List<SaleItem> saleItems)
		{
			return saleItems.Sum(item => SingleDiscountSum(item));
		}

		public static decimal SingleDiscountSum(SaleItem saleItem)
		{
			return saleItem.DiscountCreditTermsAmount + saleItem.DiscountPaymentMethodAmount + saleItem.DiscountProductTypeAmount;
		}
	}


}
