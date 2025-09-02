using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace apiTecnica.Models;

public partial class PruebaTecnicaContext : DbContext
{
    public PruebaTecnicaContext()
    {
    }

    public PruebaTecnicaContext(DbContextOptions<PruebaTecnicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CreditTerm> CreditTerms { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerType> CustomerTypes { get; set; }

    public virtual DbSet<DiscountByPaymentMethod> DiscountByPaymentMethods { get; set; }

    public virtual DbSet<DiscountByProductType> DiscountByProductTypes { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleItem> SaleItems { get; set; }

    public virtual DbSet<TaxRate> TaxRates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=pruebaTecnica;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.4.6-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CreditTerm>(entity =>
        {
            entity.HasKey(e => e.CreditTermsId).HasName("PRIMARY");

            entity.ToTable("credit_terms");

            entity.HasIndex(e => e.Days, "days").IsUnique();

            entity.Property(e => e.CreditTermsId)
                .ValueGeneratedNever()
                .HasColumnName("credit_terms_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.Days).HasColumnName("days");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.DiscountPercent)
                .HasPrecision(5, 2)
                .HasColumnName("discount_percent");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

            entity.ToTable("customer");

            entity.HasIndex(e => e.CreditTermsId, "fk_customer_credit");

            entity.HasIndex(e => e.CustomerTypeId, "fk_customer_type");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.CreditTermsId).HasColumnName("credit_terms_id");
            entity.Property(e => e.CustomerTypeId).HasColumnName("customer_type_id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");

            entity.HasOne(d => d.CreditTerms).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CreditTermsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_customer_credit");

            entity.HasOne(d => d.CustomerType).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CustomerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_customer_type");
        });

        modelBuilder.Entity<CustomerType>(entity =>
        {
            entity.HasKey(e => e.CustomerTypeId).HasName("PRIMARY");

            entity.ToTable("customer_type");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.CustomerTypeId)
                .ValueGeneratedNever()
                .HasColumnName("customer_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");
        });

        modelBuilder.Entity<DiscountByPaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PRIMARY");

            entity.ToTable("discount_by_payment_method");

            entity.Property(e => e.PaymentMethodId)
                .ValueGeneratedNever()
                .HasColumnName("payment_method_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.DiscountPercent)
                .HasPrecision(5, 2)
                .HasColumnName("discount_percent");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");

            entity.HasOne(d => d.PaymentMethod).WithOne(p => p.DiscountByPaymentMethod)
                .HasForeignKey<DiscountByPaymentMethod>(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dbpm_method");
        });

        modelBuilder.Entity<DiscountByProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PRIMARY");

            entity.ToTable("discount_by_product_type");

            entity.Property(e => e.ProductTypeId)
                .ValueGeneratedNever()
                .HasColumnName("product_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.DiscountPercent)
                .HasPrecision(5, 2)
                .HasColumnName("discount_percent");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");

            entity.HasOne(d => d.ProductType).WithOne(p => p.DiscountByProductType)
                .HasForeignKey<DiscountByProductType>(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dbpt_type");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PRIMARY");

            entity.ToTable("payment_method");

            entity.HasIndex(e => e.Code, "code").IsUnique();

            entity.Property(e => e.PaymentMethodId)
                .ValueGeneratedNever()
                .HasColumnName("payment_method_id");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(50)
                .HasColumnName("display_name");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.ProductTypeId, "fk_product_type");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.ListPrice)
                .HasPrecision(12, 2)
                .HasColumnName("list_price");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.ProductTypeId).HasColumnName("product_type_id");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_type");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PRIMARY");

            entity.ToTable("product_type");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.ProductTypeId)
                .ValueGeneratedNever()
                .HasColumnName("product_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PRIMARY");

            entity.ToTable("sale");

            entity.HasIndex(e => new { e.CustomerId, e.SaleDatetime }, "idx_sale_customer");

            entity.HasIndex(e => e.PaymentMethodId, "idx_sale_payment_method");

            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.SaleDatetime)
                .HasColumnType("datetime")
                .HasColumnName("sale_datetime");
            entity.Property(e => e.SubtotalAmount)
                .HasPrecision(14, 2)
                .HasColumnName("subtotal_amount");
            entity.Property(e => e.TaxAmount)
                .HasPrecision(14, 2)
                .HasColumnName("tax_amount");
            entity.Property(e => e.TaxRatePercent)
                .HasPrecision(5, 2)
                .HasColumnName("tax_rate_percent");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(14, 2)
                .HasColumnName("total_amount");
            entity.Property(e => e.TotalDiscountsAmount)
                .HasPrecision(14, 2)
                .HasColumnName("total_discounts_amount");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sale_customer");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Sales)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sale_payment");
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(e => e.SaleItemId).HasName("PRIMARY");

            entity.ToTable("sale_item");

            entity.HasIndex(e => e.ProductId, "idx_sale_item_product");

            entity.HasIndex(e => e.SaleId, "idx_sale_item_sale");

            entity.Property(e => e.SaleItemId).HasColumnName("sale_item_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.DiscountCreditTermsAmount)
                .HasPrecision(12, 2)
                .HasColumnName("discount_credit_terms_amount");
            entity.Property(e => e.DiscountPaymentMethodAmount)
                .HasPrecision(12, 2)
                .HasColumnName("discount_payment_method_amount");
            entity.Property(e => e.DiscountProductTypeAmount)
                .HasPrecision(12, 2)
                .HasColumnName("discount_product_type_amount");
            entity.Property(e => e.LineSubtotalAfterDiscounts)
                .HasPrecision(12, 2)
                .HasColumnName("line_subtotal_after_discounts");
            entity.Property(e => e.ListPriceAtSale)
                .HasPrecision(12, 2)
                .HasColumnName("list_price_at_sale");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(12, 3)
                .HasColumnName("quantity");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sale_item_product");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sale_item_sale");
        });

        modelBuilder.Entity<TaxRate>(entity =>
        {
            entity.HasKey(e => e.TaxRateId).HasName("PRIMARY");

            entity.ToTable("tax_rate");

            entity.Property(e => e.TaxRateId)
                .ValueGeneratedNever()
                .HasColumnName("tax_rate_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedIp)
                .HasMaxLength(45)
                .HasColumnName("created_ip");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedIp)
                .HasMaxLength(45)
                .HasColumnName("deleted_ip");
            entity.Property(e => e.EffectiveFrom).HasColumnName("effective_from");
            entity.Property(e => e.EffectiveTo).HasColumnName("effective_to");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.RatePercent)
                .HasPrecision(5, 2)
                .HasColumnName("rate_percent");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedIp)
                .HasMaxLength(45)
                .HasColumnName("updated_ip");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
