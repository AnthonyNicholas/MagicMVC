﻿// <auto-generated />
using MagicMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace MagicMVC.Migrations
{
    [DbContext(typeof(MagicMVCContext))]
    partial class MagicMVCContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MagicMVC.Models.Franchisee", b =>
                {
                    b.Property<string>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("StoreID");

                    b.HasKey("UserID");

                    b.ToTable("Franchisees");
                });

            modelBuilder.Entity("MagicMVC.Models.OwnerInventory", b =>
                {
                    b.Property<int>("ProductID");

                    b.Property<int>("StockLevel");

                    b.HasKey("ProductID");

                    b.ToTable("OwnerInventory");
                });

            modelBuilder.Entity("MagicMVC.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.HasKey("ProductID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MagicMVC.Models.StockRequest", b =>
                {
                    b.Property<int>("StockRequestID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProductID");

                    b.Property<int>("Quantity");

                    b.Property<int>("StoreID");

                    b.HasKey("StockRequestID");

                    b.HasIndex("ProductID");

                    b.HasIndex("StoreID", "ProductID");

                    b.ToTable("StockRequests");
                });

            modelBuilder.Entity("MagicMVC.Models.Store", b =>
                {
                    b.Property<int>("StoreID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("StoreID");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("MagicMVC.Models.StoreInventory", b =>
                {
                    b.Property<int>("StoreID");

                    b.Property<int>("ProductID");

                    b.Property<int>("StockLevel");

                    b.HasKey("StoreID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("StoreInventory");
                });

            modelBuilder.Entity("MagicMVC.Models.OwnerInventory", b =>
                {
                    b.HasOne("MagicMVC.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MagicMVC.Models.StockRequest", b =>
                {
                    b.HasOne("MagicMVC.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MagicMVC.Models.Store", "Store")
                        .WithMany("StockRequestList")
                        .HasForeignKey("StoreID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MagicMVC.Models.StoreInventory", "StoreInventory")
                        .WithMany()
                        .HasForeignKey("StoreID", "ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MagicMVC.Models.StoreInventory", b =>
                {
                    b.HasOne("MagicMVC.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MagicMVC.Models.Store", "Store")
                        .WithMany("StoreInventoryList")
                        .HasForeignKey("StoreID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
