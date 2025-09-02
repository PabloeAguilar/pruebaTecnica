using System;
using System.Collections.Generic;

namespace apiTecnica.Models;

public partial class Product
{
    public long ProductId { get; set; }

    public string Name { get; set; } = null!;

    public sbyte ProductTypeId { get; set; }

    public decimal ListPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedIp { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedIp { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedIp { get; set; }

    public virtual ProductType ProductType { get; set; } = null!;

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
