using System;
using System.Collections.Generic;

namespace test_demo.Models;

public partial class Deliverypoint
{
    public int Deliverypointid { get; set; }

    public string? Deliverypointname { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
