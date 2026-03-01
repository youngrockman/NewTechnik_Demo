using System;
using System.Collections.Generic;

namespace test_demo.Models;

public partial class Supplier
{
    public int Supplierid { get; set; }

    public string? Suppliername { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
