﻿using Microsoft.EntityFrameworkCore;
using MagicMVC.Models;

namespace MagicMVC.Models
{
    public class MagicMVCContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<OwnerInventory> OwnerInventory { get; set; }
        public DbSet<StockRequest> StockRequests { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Order> Orders { get; set; }

        public MagicMVCContext(DbContextOptions<MagicMVCContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreInventory>().HasKey(x => new { x.StoreID, x.ProductID });
        }

        public DbSet<MagicMVC.Models.StoreInventory> StoreInventory { get; set; }
    }
}
