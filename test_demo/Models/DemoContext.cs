using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace test_demo.Models;

public partial class DemoContext : DbContext
{
    public DemoContext()
    {
    }

    public DemoContext(DbContextOptions<DemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Deliverypoint> Deliverypoints { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderproduct> Orderproducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productcategory> Productcategories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;username=postgres;password=1234;port=5432;database=demo");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Deliverypoint>(entity =>
        {
            entity.HasKey(e => e.Deliverypointid).HasName("deliverypoint_pkey");

            entity.ToTable("deliverypoint");

            entity.Property(e => e.Deliverypointid).HasColumnName("deliverypointid");
            entity.Property(e => e.Deliverypointname).HasColumnName("deliverypointname");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Manufacturerid).HasName("manufacturer_pkey");

            entity.ToTable("manufacturer");

            entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
            entity.Property(e => e.Manufacturername).HasColumnName("manufacturername");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Ordercreatedate).HasColumnName("ordercreatedate");
            entity.Property(e => e.Orderdeliverydate).HasColumnName("orderdeliverydate");
            entity.Property(e => e.Orderpickuppoint).HasColumnName("orderpickuppoint");
            entity.Property(e => e.Orderstatus).HasColumnName("orderstatus");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.OrderpickuppointNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderpickuppoint)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_deliverypoint_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("order_user_fk");
        });

        modelBuilder.Entity<Orderproduct>(entity =>
        {
            entity.HasKey(e => e.Orderproductid).HasName("orderproduct_pkey");

            entity.ToTable("orderproduct");

            entity.HasIndex(e => new { e.Orderid, e.Productarticlenumber }, "orderproduct_orderid_productarticlenumber_key").IsUnique();

            entity.Property(e => e.Orderproductid).HasColumnName("orderproductid");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_orderid_fkey");

            entity.HasOne(d => d.ProductarticlenumberNavigation).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.Productarticlenumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_productarticlenumber_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productarticlenumber).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Maxproductdiscount).HasColumnName("maxproductdiscount");
            entity.Property(e => e.Productcategory).HasColumnName("productcategory");
            entity.Property(e => e.Productcost)
                .HasPrecision(10, 4)
                .HasColumnName("productcost");
            entity.Property(e => e.Productdescription).HasColumnName("productdescription");
            entity.Property(e => e.Productdiscount)
                .HasPrecision(10, 2)
                .HasColumnName("productdiscount");
            entity.Property(e => e.Productdiscountamount).HasColumnName("productdiscountamount");
            entity.Property(e => e.Productmanufacturer).HasColumnName("productmanufacturer");
            entity.Property(e => e.Productname).HasColumnName("productname");
            entity.Property(e => e.Productphoto).HasColumnName("productphoto");
            entity.Property(e => e.Productquantityinstock).HasColumnName("productquantityinstock");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Unit).HasColumnName("unit");

            entity.HasOne(d => d.ProductcategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productcategory)
                .HasConstraintName("product_productcategory_fk");

            entity.HasOne(d => d.ProductmanufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productmanufacturer)
                .HasConstraintName("product_manufacturer_fk");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.Supplierid)
                .HasConstraintName("product_supplier_fk");
        });

        modelBuilder.Entity<Productcategory>(entity =>
        {
            entity.HasKey(e => e.Productcategoryid).HasName("productcategory_pkey");

            entity.ToTable("productcategory");

            entity.Property(e => e.Productcategoryid).HasColumnName("productcategoryid");
            entity.Property(e => e.Productcategoryname).HasColumnName("productcategoryname");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Supplierid).HasName("supplier_pkey");

            entity.ToTable("supplier");

            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Suppliername).HasColumnName("suppliername");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Userlogin).HasColumnName("userlogin");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Userpassword).HasColumnName("userpassword");
            entity.Property(e => e.Userpatronymic)
                .HasMaxLength(100)
                .HasColumnName("userpatronymic");
            entity.Property(e => e.Userrole).HasColumnName("userrole");
            entity.Property(e => e.Usersurname)
                .HasMaxLength(100)
                .HasColumnName("usersurname");

            entity.HasOne(d => d.UserroleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Userrole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_userrole_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
