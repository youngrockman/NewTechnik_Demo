using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace test_demo.Models;

public partial class Product
{
    public string Productarticlenumber { get; set; } = null!;

    public string Productname { get; set; } = null!;

    public string Productdescription { get; set; } = null!;

    public int Productcategory { get; set; }

    public string? Productphoto { get; set; }

    public Bitmap ParseImage {
        get
        {
            try
            {
                if (string.IsNullOrEmpty(Productphoto))
                {
                    return new Bitmap (AppDomain.CurrentDomain.BaseDirectory + "/Images/picture.png");
                }

                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/Images/" + Productphoto);
            }
            catch (Exception ex) { return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/Images/picture.png"); }



        }
    }

    public int Productmanufacturer { get; set; }

    public decimal Productcost { get; set; }

    public int? Productdiscountamount { get; set; }

    public int Productquantityinstock { get; set; }

    public decimal? Productdiscount { get; set; }

    public int Supplierid { get; set; }

    public int? Maxproductdiscount { get; set; }

    public string? Unit { get; set; }

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual Productcategory ProductcategoryNavigation { get; set; } = null!;

    public virtual Manufacturer ProductmanufacturerNavigation { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
