using System;
using System.Collections.Generic;

namespace apiTecnica.Models;

public partial class SaleItem
{
    public long SaleItemId { get; set; }

    public long SaleId { get; set; }

    public long ProductId { get; set; }

    public decimal Quantity { get; set; }

    public decimal ListPriceAtSale { get; set; }

    public decimal DiscountProductTypeAmount { get; set; }

    public decimal DiscountPaymentMethodAmount { get; set; }

    public decimal DiscountCreditTermsAmount { get; set; }

    public decimal LineSubtotalAfterDiscounts { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedIp { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedIp { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedIp { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
