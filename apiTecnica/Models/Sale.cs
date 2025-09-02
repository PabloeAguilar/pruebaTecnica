using System;
using System.Collections.Generic;

namespace apiTecnica.Models;

public partial class Sale
{
    public long SaleId { get; set; }

    public long CustomerId { get; set; }

    public sbyte PaymentMethodId { get; set; }

    public decimal TaxRatePercent { get; set; }

    public decimal SubtotalAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal TotalDiscountsAmount { get; set; }

    public DateTime SaleDatetime { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedIp { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedIp { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedIp { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
