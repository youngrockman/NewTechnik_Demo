using System;
using System.Collections.Generic;

namespace test_demo.Models;

public partial class Productcategory
{
    public int Productcategoryid { get; set; }

    public string? Productcategoryname { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
