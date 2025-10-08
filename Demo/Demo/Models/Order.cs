using System;
using System.Collections.Generic;

namespace Demo.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public decimal? Amount { get; set; }

    public DateOnly? OrderDate { get; set; }

    public virtual Customer? Customer { get; set; }
}
