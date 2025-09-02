using System;
using System.Collections.Generic;

namespace apiTecnica.Models;

public partial class CreditTerm
{
    public sbyte CreditTermsId { get; set; }

    public short? Days { get; set; }

    public decimal DiscountPercent { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedIp { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedIp { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedIp { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
