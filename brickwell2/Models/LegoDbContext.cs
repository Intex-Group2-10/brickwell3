﻿using System;
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

    public virtual DbSet<FraudPrediction> FraudPredictions { get; set; }

    public virtual DbSet<ItemBasedRecommendation> ItemBasedRecommendations { get; set; }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }


    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<UserBasedRecommendation> UserBasedRecommendations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:brickwellserver.database.windows.net,1433;Initial Catalog=brickwellDB_2024-04-15T22-45Z;Persist Security Info=False;User ID=section2group10;Password=wewillwin210@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CountryOfResidence).HasColumnName("country_of_residence");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.LastName).HasColumnName("last_name");
        });

        modelBuilder.Entity<FraudPrediction>(entity =>
        {
            entity.HasKey(e => e.TransactionID);

            entity.ToTable("fraud_prediction");

            entity.Property(e => e.TransactionID)
                .ValueGeneratedOnAdd()
                .HasColumnName("transaction_ID");

            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BankHalifax).HasColumnName("bank_Halifax");
            entity.Property(e => e.BankHsbc).HasColumnName("bank_HSBC");
            entity.Property(e => e.BankLloyds).HasColumnName("bank_Lloyds");
            entity.Property(e => e.BankMetro).HasColumnName("bank_Metro");
            entity.Property(e => e.BankMonzo).HasColumnName("bank_Monzo");
            entity.Property(e => e.BankRbs).HasColumnName("bank_RBS");
            entity.Property(e => e.CountryOfTransactionIndia).HasColumnName("country_of_transaction_India");
            entity.Property(e => e.CountryOfTransactionRussia).HasColumnName("country_of_transaction_Russia");
            // entity.Property(e => e.CountryOfTransactionUnitedKingdom).HasColumnName("country_of_transaction_UnitedKingdom");
            entity.Property(e => e.CountryOfTransactionUsa).HasColumnName("country_of_transaction_USA");
            // entity.Property(e => e.DayOfWeekMon).HasColumnName("day_of_week_Mon");
            // entity.Property(e => e.DayOfWeekSat).HasColumnName("day_of_week_Sat");
            // entity.Property(e => e.DayOfWeekSun).HasColumnName("day_of_week_Sun");
            // entity.Property(e => e.DayOfWeekThu).HasColumnName("day_of_week_Thu");
            // entity.Property(e => e.DayOfWeekTue).HasColumnName("day_of_week_Tue");
            // entity.Property(e => e.DayOfWeekWed).HasColumnName("day_of_week_Wed");
            entity.Property(e => e.EntryModePin).HasColumnName("entry_mode_PIN");
            entity.Property(e => e.EntryModeTap).HasColumnName("entry_mode_Tap");
            entity.Property(e => e.ShippingAddressIndia).HasColumnName("shipping_address_India");
            entity.Property(e => e.ShippingAddressRussia).HasColumnName("shipping_address_Russia");
            // entity.Property(e => e.ShippingAddressUnitedKingdom).HasColumnName("shipping_address_UnitedKingdom");
            entity.Property(e => e.ShippingAddressUsa).HasColumnName("shipping_address_USA");
            // entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.TypeOfCardVisa).HasColumnName("type_of_card_Visa");
            entity.Property(e => e.TypeOfTransactionOnline).HasColumnName("type_of_transaction_Online");
            entity.Property(e => e.TypeOfTransactionPos).HasColumnName("type_of_transaction_POS");
        });

        modelBuilder.Entity<ItemBasedRecommendation>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("item_based_recommendations");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_ID");
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
            entity.HasKey(e => new { e.TransactionId, e.ProductId });

            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Rating).HasColumnName("rating");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");
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
            entity.Property(e => e.TypeOfCard).HasColumnName("type_of_card");
            entity.Property(e => e.TypeOfTransaction).HasColumnName("type_of_transaction");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImgLink).HasColumnName("img_link");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NumParts).HasColumnName("num_parts");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PrimaryColor).HasColumnName("primary_color");
            entity.Property(e => e.SecondaryColor).HasColumnName("secondary_color");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<UserBasedRecommendation>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("user_based_recommendations");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_ID");
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
