﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Models.Builders;
using WarehouseManagementSystem.Models.Entities;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Models
{
    public class WarehouseDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ZoneCategory> ZoneCategories { get; set; }
        public DbSet<MovementHistory> MovementHistories { get; set; }
        public DbSet<ZonePosition> ZonePositions { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentItem> ShipmentItems { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<ProductInZonePosition> ProductInZonePositions { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

        public WarehouseDbContext()
        {
        }
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) 
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                string? connectionString = configuration.GetConnectionString("CurrentConnection");

                if (!ConnectionTester.TestConnectionString(connectionString))
                {
                    connectionString = configuration.GetConnectionString("DefaultConnection")
                                        ?? throw new NullReferenceException("Connection string == null");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ShipmentItems)
                .WithOne(si => si.Product)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ReceiptItems)
                .WithOne(ri => ri.Product)
                .HasForeignKey(ri => ri.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Labels)
                .WithOne(l => l.Product)
                .HasForeignKey(l => l.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Warehouse)
                .WithMany()
                .HasForeignKey(p => p.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Warehouse>()
                .HasMany(w => w.Products)
                .WithOne(p => p.Warehouse)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovementHistory>()
                .HasOne(m => m.SourceZonePosition)
                .WithMany()
                .HasForeignKey(m => m.SourceZonePositionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovementHistory>()
                .HasOne(m => m.DestinationZonePosition)
                .WithMany()
                .HasForeignKey(m => m.DestinationZonePositionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ZonePosition>()
                .HasMany(zp => zp.ProductsInZonePosition)
                .WithOne(p => p.ZonePosition)
                .HasForeignKey(p => p.ZonePositionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ZonePosition>()
                .HasMany(zp => zp.SourceMovementHistories)
                .WithOne(mh => mh.SourceZonePosition)
                .HasForeignKey(mh => mh.SourceZonePositionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ZonePosition>()
                .HasMany(zp => zp.DestinationMovementHistories)
                .WithOne(mh => mh.DestinationZonePosition)
                .HasForeignKey(mh => mh.DestinationZonePositionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Zone>()
                .HasMany(z => z.ZonePositions)
                .WithOne(zp => zp.Zone)
                .HasForeignKey(zp => zp.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Zone>()
                .HasOne(z => z.ZoneCategory)
                .WithMany()
                .HasForeignKey(z => z.ZoneCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ZoneCategory>()
                .HasMany(zc => zc.Zones)
                .WithOne(z => z.ZoneCategory)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Shipments)
                .WithOne(s => s.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Receipts)
                .WithOne(r => r.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reports)
                .WithOne(r => r.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Shipment>()
                .HasMany(s => s.ShipmentItems)
                .WithOne(si => si.Shipment)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShipmentItem>()
                .HasOne(si => si.Shipment)
                .WithMany(s => s.ShipmentItems)
                .HasForeignKey(si => si.ShipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShipmentItem>()
                .HasOne(si => si.Product)
                .WithMany(p => p.ShipmentItems)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.Supplier)
                .WithMany()
                .HasForeignKey(r => r.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.User)
                .WithMany(u => u.Receipts)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReceiptItem>()
                .HasOne(ri => ri.Receipt)
                .WithMany(r => r.ReceiptItems)
                .HasForeignKey(ri => ri.ReceiptId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReceiptItem>()
                .HasOne(ri => ri.Product)
                .WithMany(p => p.ReceiptItems)
                .HasForeignKey(ri => ri.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Warehouse>()
                .HasMany(w => w.Zones)
                .WithOne(z => z.Warehouse)
                .HasForeignKey(z => z.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.Address)
                .WithMany()
                .HasForeignKey(w => w.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Receipts)
                .WithOne(r => r.Supplier)
                .HasForeignKey(r => r.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Supplier>()
                .HasOne(s => s.Address)
                .WithMany()
                .HasForeignKey(s => s.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Shipments)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Address)
                .WithMany()
                .HasForeignKey(c => c.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Address>()
                .HasMany(a => a.Warehouses)
                .WithOne(w => w.Address)
                .HasForeignKey(w => w.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Address>()
                .HasMany(a => a.Suppliers)
                .WithOne(s => s.Address)
                .HasForeignKey(s => s.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Address>()
                .HasMany(a => a.Customers)
                .WithOne(c => c.Address)
                .HasForeignKey(c => c.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Address>()
                .HasMany(a => a.Manufacturers)
                .WithOne(m => m.Address)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductInZonePosition>()
                .HasOne(pzp => pzp.ZonePosition)
                .WithMany(zp => zp.ProductsInZonePosition)
                .HasForeignKey(pzp => pzp.ZonePositionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Manufacturer>()
                .HasOne(m => m.Address)
                .WithMany(a => a.Manufacturers)
                .HasForeignKey(m => m.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Label>()
                .HasOne(l => l.Product)
                .WithMany(p => p.Labels)
                .HasForeignKey(l => l.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductPhotos)
                .WithOne(pp => pp.Product)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductsInZonePositions)
                .WithOne(pzp => pzp.Product)
                .HasForeignKey(pzp => pzp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.MovementHistories)
                .WithOne(mh => mh.Product)
                .HasForeignKey(mh => mh.ProductId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Customer>()
                .Property(c => c.DiscountPercentage)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.DiscountPercentage)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Quantity)
                .HasColumnType("decimal(18,2)");
        }
    }
}
