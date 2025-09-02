using System;
using System.Collections.Generic;

namespace apiTecnica.Models;

public partial class PaymentMethod
{
    public sbyte PaymentMethodId { get; set; }

    public string? Code { get; set; }

    public string? DisplayName { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedIp { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedIp { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedIp { get; set; }

    public virtual DiscountByPaymentMethod? DiscountByPaymentMethod { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
