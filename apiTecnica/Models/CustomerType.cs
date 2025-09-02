using System;
using System.Collections.Generic;

namespace apiTecnica.Models;

public partial class CustomerType
{
    public sbyte CustomerTypeId { get; set; }

    public string? Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedIp { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedIp { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedIp { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
