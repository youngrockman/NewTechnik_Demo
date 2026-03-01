using System;
using System.Collections.Generic;

namespace test_demo.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public string Orderstatus { get; set; } = null!;

    public DateOnly? Ordercreatedate { get; set; }

    public DateOnly? Orderdeliverydate { get; set; }

    public int? Orderpickuppoint { get; set; }

    public int? Code { get; set; }

    public int Userid { get; set; }

    public virtual Deliverypoint? OrderpickuppointNavigation { get; set; }

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual User User { get; set; } = null!;
}
