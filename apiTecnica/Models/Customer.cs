using System;
using System.Collections.Generic;

namespace apiTecnica.Models;

public partial class Customer
{
    public long CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public sbyte CustomerTypeId { get; set; }

    public sbyte CreditTermsId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedIp { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedIp { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedIp { get; set; }

    public virtual CreditTerm CreditTerms { get; set; } = null!;

    public virtual CustomerType CustomerType { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
