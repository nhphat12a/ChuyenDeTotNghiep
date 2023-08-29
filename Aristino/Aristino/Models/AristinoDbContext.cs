using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Aristino.Models;

public partial class AristinoDbContext : DbContext
{
    public AristinoDbContext()
    {
    }

    public AristinoDbContext(DbContextOptions<AristinoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AboutU> AboutUs { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AristinoPolicy> AristinoPolicies { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DiscountBanner> DiscountBanners { get; set; }

    public virtual DbSet<FashionCollection> FashionCollections { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<MostSoldProduct> MostSoldProducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrdersDetail> OrdersDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<TransactionStatus> TransactionStatuses { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserStatus> UserStatuses { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-2J5U7E9\\SQLEXPRESS;Database=AristinoDB;Integrated Security=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AboutU>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AboutUs__3214EC27E85396A4");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Province).HasMaxLength(100);
            entity.Property(e => e.ShopAddress).HasMaxLength(500);
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA586FC9DB95F");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Passwords).HasMaxLength(100);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Customer).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_AccountsCustomers");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountsUserRole");
        });

        modelBuilder.Entity<AristinoPolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__Aristino__2E1339442946A942");

            entity.ToTable("AristinoPolicy");

            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.PolicyTitle).HasMaxLength(200);
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blog__54379E3060FF1FCC");

            entity.ToTable("Blog");

            entity.Property(e => e.BlogTitle).HasMaxLength(200);
            entity.Property(e => e.PostDate).HasMaxLength(50);
            entity.Property(e => e.SourceName).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_BlogCustomers");
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.BlogCommentId).HasName("PK__BlogComm__5F60E1D1DA8B8884");

            entity.Property(e => e.CommentDate).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK_BlogCommentsBlog");

            entity.HasOne(d => d.Customer).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_BlogComments");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD7B77F98446F");

            entity.ToTable("Cart");

            entity.Property(e => e.Color).HasMaxLength(10);
            entity.Property(e => e.Size).HasMaxLength(10);

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_CartCustomers");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_CartProducts");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B10841C7E");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryDescription).HasMaxLength(50);
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Color__8DA7676D085592C2");

            entity.ToTable("Color");

            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.ColorName).HasMaxLength(50);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFCA6F26A0F8");

            entity.Property(e => e.CommentContent).HasMaxLength(300);
            entity.Property(e => e.CommentedDate).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Customer).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommentsCustomer");

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_CommentsProducts");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomersId).HasName("PK__Customer__EB5B581EB5DDC761");

            entity.Property(e => e.CustomersId).HasColumnName("CustomersID");
            entity.Property(e => e.CardNumber).HasMaxLength(16);
            entity.Property(e => e.CardOwner).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(20);
            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.CustomerAddress).HasMaxLength(100);
            entity.Property(e => e.Cvc).HasColumnName("CVC");
            entity.Property(e => e.Dob).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.ExpiredDate).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.Gender).WithMany(p => p.Customers)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("FK_CustomersGender");

            entity.HasOne(d => d.Status).WithMany(p => p.Customers)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_CustomersUserStatus");
        });

        modelBuilder.Entity<DiscountBanner>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__Discount__E43F6DF64C427A65");

            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.DiscountName).HasMaxLength(100);
            entity.Property(e => e.EndSale).HasMaxLength(100);
            entity.Property(e => e.StartSale).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.DiscountBanners)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_DiscountBannersCategory");
        });

        modelBuilder.Entity<FashionCollection>(entity =>
        {
            entity.HasKey(e => e.CollectionId).HasName("PK__FashionC__7DE6BC24DB11F19B");

            entity.ToTable("FashionCollection");

            entity.Property(e => e.CollectionId).HasColumnName("CollectionID");
            entity.Property(e => e.CollectionDes).HasMaxLength(1000);
            entity.Property(e => e.CollectionName).HasMaxLength(200);
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__Gender__4E24E9F76ECA1771");

            entity.ToTable("Gender");

            entity.Property(e => e.GenderName).HasMaxLength(10);
        });

        modelBuilder.Entity<MostSoldProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MostSold__3213E83FEA609FDC");

            entity.ToTable("MostSoldProduct");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsReset).HasColumnName("isReset");

            entity.HasOne(d => d.Product).WithMany(p => p.MostSoldProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_MostSoldProductProduct");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAFA84BBBE7");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.IsPaid).HasColumnName("isPaid");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderNumber).HasMaxLength(100);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.ShipDate).HasColumnType("datetime");
            entity.Property(e => e.TransacId).HasColumnName("TransacID");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdersCustomers");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdersPayment");

            entity.HasOne(d => d.Transac).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TransacId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdersTransactionStatus");
        });

        modelBuilder.Entity<OrdersDetail>(entity =>
        {
            entity.HasKey(e => e.OdersDetailId).HasName("PK__OrdersDe__E2D303FC3CF16DC8");

            entity.ToTable("OrdersDetail");

            entity.Property(e => e.OdersDetailId).HasColumnName("OdersDetailID");
            entity.Property(e => e.Color).HasMaxLength(10);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Size).HasMaxLength(10);

            entity.HasOne(d => d.Order).WithMany(p => p.OrdersDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdersDetailOrders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrdersDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdersDetailProducts");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A58480D1846");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.PaymentName).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDC2DD9614");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CollectionId).HasColumnName("CollectionID");
            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.ProductImage).HasMaxLength(100);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.ShortDes).HasMaxLength(200);
            entity.Property(e => e.StarRate).HasMaxLength(10);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductsCategories");

            entity.HasOne(d => d.Collection).WithMany(p => p.Products)
                .HasForeignKey(d => d.CollectionId)
                .HasConstraintName("FK_ProductsFashionCollection");

            entity.HasOne(d => d.DiscountNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK_ProductsDiscountBanner");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__Size__83BD095A1DE77859");

            entity.ToTable("Size");

            entity.Property(e => e.SizeId).HasColumnName("SizeID");
            entity.Property(e => e.SizeName).HasMaxLength(10);
        });

        modelBuilder.Entity<TransactionStatus>(entity =>
        {
            entity.HasKey(e => e.TransacId).HasName("PK__Transact__FDD99D59D72AD473");

            entity.ToTable("TransactionStatus");

            entity.Property(e => e.TransacId).HasColumnName("TransacID");
            entity.Property(e => e.TransacDes).HasMaxLength(100);
            entity.Property(e => e.TransacName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__UserRole__8AFACE3A692F83BF");

            entity.ToTable("UserRole");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(20);
        });

        modelBuilder.Entity<UserStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__UserStat__C8EE20633FBC5A4A");

            entity.ToTable("UserStatus");

            entity.Property(e => e.StatusName).HasMaxLength(20);
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("PK__Wishlist__233189CB6ED5908A");

            entity.ToTable("Wishlist");

            entity.Property(e => e.WishlistId).HasColumnName("WishlistID");
            entity.Property(e => e.Color).HasMaxLength(10);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Size).HasMaxLength(10);

            entity.HasOne(d => d.Customer).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_WishlistCustomers");

            entity.HasOne(d => d.Product).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_WishlistProducts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
