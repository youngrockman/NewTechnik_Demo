using System;
using System.Collections.Generic;

namespace test_demo.Models;

public partial class Manufacturer
{
    public int Manufacturerid { get; set; }

    public string? Manufacturername { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
