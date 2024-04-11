using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace brickwell2.Models;

public partial class LegoDbContext : DbContext
{
    public LegoDbContext()
    {
    }

    public LegoDbContext(DbContextOptions<LegoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<ItemBasedRecommendation> ItemBasedRecommendations { get; set; }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<UserBasedRecommendation> UserBasedRecommendations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=LegoDB.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            // entity.HasNoKey();

            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CountryOfResidence).HasColumnName("country_of_residence");
            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.LastName).HasColumnName("last_name");
        });

        modelBuilder.Entity<ItemBasedRecommendation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("item_based_recommendations");

            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Recommendation1).HasColumnName("Recommendation_1");
            entity.Property(e => e.Recommendation10).HasColumnName("Recommendation_10");
            entity.Property(e => e.Recommendation2).HasColumnName("Recommendation_2");
            entity.Property(e => e.Recommendation3).HasColumnName("Recommendation_3");
            entity.Property(e => e.Recommendation4).HasColumnName("Recommendation_4");
            entity.Property(e => e.Recommendation5).HasColumnName("Recommendation_5");
            entity.Property(e => e.Recommendation6).HasColumnName("Recommendation_6");
            entity.Property(e => e.Recommendation7).HasColumnName("Recommendation_7");
            entity.Property(e => e.Recommendation8).HasColumnName("Recommendation_8");
            entity.Property(e => e.Recommendation9).HasColumnName("Recommendation_9");
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Bank).HasColumnName("bank");
            entity.Property(e => e.CountryOfTransaction).HasColumnName("country_of_transaction");
            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.EntryMode).HasColumnName("entry_mode");
            entity.Property(e => e.Fraud).HasColumnName("fraud");
            entity.Property(e => e.ShippingAddress).HasColumnName("shipping_address");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");
            entity.Property(e => e.TypeOfCard).HasColumnName("type_of_card");
            entity.Property(e => e.TypeOfTransaction).HasColumnName("type_of_transaction");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            // entity.HasNoKey();

            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImgLink).HasColumnName("img_link");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NumParts).HasColumnName("num_parts");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PrimaryColor).HasColumnName("primary_color");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.SecondaryColor).HasColumnName("secondary_color");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<UserBasedRecommendation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("user_based_recommendations");

            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.RecommendedProduct1).HasColumnName("Recommended_Product_1");
            entity.Property(e => e.RecommendedProduct10).HasColumnName("Recommended_Product_10");
            entity.Property(e => e.RecommendedProduct2).HasColumnName("Recommended_Product_2");
            entity.Property(e => e.RecommendedProduct3).HasColumnName("Recommended_Product_3");
            entity.Property(e => e.RecommendedProduct4).HasColumnName("Recommended_Product_4");
            entity.Property(e => e.RecommendedProduct5).HasColumnName("Recommended_Product_5");
            entity.Property(e => e.RecommendedProduct6).HasColumnName("Recommended_Product_6");
            entity.Property(e => e.RecommendedProduct7).HasColumnName("Recommended_Product_7");
            entity.Property(e => e.RecommendedProduct8).HasColumnName("Recommended_Product_8");
            entity.Property(e => e.RecommendedProduct9).HasColumnName("Recommended_Product_9");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
