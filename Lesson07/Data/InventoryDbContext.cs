using Lesson07.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace Lesson07.Data
{
    public class InventoryDbContext : DbContext
    {
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Supply> Supplies { get; set; }
        public virtual DbSet<SupplyProduct> SupplyProducts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SaleProduct> SalesProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=WIN-37KMC9AKIMF;Initial Catalog=InventoryManagement;Integrated Security=True;Encrypt=False");
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Category
            
            modelBuilder.Entity<Category>()
                .ToTable(nameof(Category));
            modelBuilder.Entity<Category>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Category>()
                .Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            #endregion

            #region Product

            modelBuilder.Entity<Product>()
                .ToTable(nameof(Product));
            modelBuilder.Entity<Product>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Product>()
                .Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);
            modelBuilder.Entity<Product>()
                .Property(x => x.Price)
                .HasColumnType("money")
                .HasDefaultValue(0)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Product>()
                .Property(x => x.ExpireDate)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<Product>()
                .HasMany(p => p.SupplyProducts)
                .WithOne(sp => sp.Product)
                .HasForeignKey(sp => sp.ProductId);
            modelBuilder.Entity<Product>()
                .HasMany(sp => sp.SaleProducts)
                .WithOne(p => p.Product);

            #endregion

            #region Supplier

            modelBuilder.Entity<Supplier>()
                .ToTable(nameof(Supplier));
            modelBuilder.Entity<Supplier>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Supplier>()
                .Property(x => x.FirstName)
                .HasMaxLength(150)
                .IsRequired();
            modelBuilder.Entity<Supplier>()
                .Property(x => x.LastName)
                .HasMaxLength(150)
                .IsRequired(false);
            modelBuilder.Entity<Supplier>()
                .Property(x => x.Company)
                .HasMaxLength(255)
                .IsRequired();
            modelBuilder.Entity<Supplier>()
                .Property(x => x.PhoneNumber)
                .HasMaxLength(13)
                .IsRequired();

            #endregion

            #region Supply

            modelBuilder.Entity<Supply>()
                .ToTable(nameof(Supply));
            modelBuilder.Entity<Supply>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Supply>()
                .Property(x => x.Date)
                .IsRequired();
            modelBuilder.Entity<Supply>()
                .Property(x => x.TotalDue)
                .HasColumnType("money")
                .HasPrecision(18, 2);
            modelBuilder.Entity<Supply>()
                .Property(x => x.TotalPaid)
                .HasColumnType("money")
                .HasPrecision(18, 2);

            modelBuilder.Entity<Supply>()
                .HasMany(x => x.SupplyProducts)
                .WithOne(sp => sp.Supply)
                .HasForeignKey(sp => sp.SupplyId);

            #endregion

            #region Supply Product

            modelBuilder.Entity<SupplyProduct>()
                .ToTable(nameof(SupplyProduct));
            modelBuilder.Entity<SupplyProduct>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<SupplyProduct>()
                .Property(x => x.Quantity)
                .IsRequired()
                .HasDefaultValue(1);
            modelBuilder.Entity<SupplyProduct>()
                .Property(x => x.UnitPrice)
                .HasColumnType("money")
                .HasPrecision(18, 2)
                .IsRequired();

            modelBuilder.Entity<SupplyProduct>()
                .HasOne(sp => sp.Supply)
                .WithMany(s => s.SupplyProducts)
                .HasForeignKey(sp => sp.SupplyId);
            modelBuilder.Entity<SupplyProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.SupplyProducts)
                .HasForeignKey(sp => sp.ProductId);

            #endregion

            #region Customer

            modelBuilder.Entity<Customer>()
                .ToTable(nameof(Customer));
            modelBuilder.Entity<Customer>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Customer>()
                .Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<Customer>()
                .Property(x => x.LastName)
                .HasMaxLength(150)
                .IsRequired();
            modelBuilder.Entity<Customer>()
                .Property(x => x.PhoneNumber)
                .HasMaxLength(13)
                .IsRequired();
            modelBuilder.Entity<Customer>()
                .Property(x => x.Address)
                .HasMaxLength(100)
                .IsRequired(false);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Sales)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerId);


            #endregion

            #region Sale

            modelBuilder.Entity <Sale>()
                .ToTable(nameof(Sale));
            modelBuilder.Entity<Sale>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Sale>()
                .Property(x => x.SaleDate)
                .IsRequired();
            modelBuilder.Entity<Sale>()
                .Property(x => x.TotalDue)
                .HasColumnType("money")
                .HasPrecision(18, 2)
                .IsRequired();
            modelBuilder.Entity<Sale>()
                .Property(x => x.TotalPaid)
                .HasColumnType("money")
                .HasPrecision(18, 2)
                .IsRequired();
            modelBuilder.Entity<Sale>()
                .Property(x => x.TotalDiscount)
                .HasPrecision(18, 2)
                .HasDefaultValue(0)
                .IsRequired();

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId);
            modelBuilder.Entity<Sale>()
                .HasMany(s => s.SaleProducts)
                .WithOne(sp => sp.Sale)
                .HasForeignKey(sp => sp.SaleId);

            #endregion

            #region SaleProduct

            modelBuilder.Entity<SaleProduct>()
                .ToTable(nameof(SaleProduct));
            modelBuilder.Entity<SaleProduct>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<SaleProduct>()
                .Property(x => x.Quantity)
                .HasDefaultValue(1);
            modelBuilder.Entity<SaleProduct>()
                .Property(x => x.UnitPrice)
                .HasColumnType("money")
                .HasPrecision(18, 2)
                .IsRequired();
            modelBuilder.Entity<SaleProduct>()
                .Property(x => x.Discount)
                .HasPrecision(18, 2)
                .HasColumnType("money")
                .IsRequired();

            modelBuilder.Entity<SaleProduct>()
                .HasOne(sp => sp.Sale)
                .WithMany(s => s.SaleProducts)
                .HasForeignKey(sp => sp.SaleId);
            modelBuilder.Entity<SaleProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.SaleProducts)
                .HasForeignKey(sp => sp.ProductId);

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
