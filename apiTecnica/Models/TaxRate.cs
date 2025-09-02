using System;
using System.Collections.Generic;

namespace apiTecnica.Models;

public partial class TaxRate
{
    public sbyte TaxRateId { get; set; }

    public string? Name { get; set; }

    public decimal RatePercent { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedIp { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedIp { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedIp { get; set; }
}
