using System;
using System.Collections.Generic;

namespace apiTecnica.Models;

public partial class DiscountByPaymentMethod
{
    public sbyte PaymentMethodId { get; set; }

    public decimal DiscountPercent { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedIp { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedIp { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedIp { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
}
