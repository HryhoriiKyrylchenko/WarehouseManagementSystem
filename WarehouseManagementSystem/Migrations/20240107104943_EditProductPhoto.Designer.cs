﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WarehouseManagementSystem.Models;

#nullable disable

namespace WarehouseManagementSystem.Migrations
{
    [DbContext(typeof(WarehouseDbContext))]
    [Migration("20240107104943_EditProductPhoto")]
    partial class EditProductPhoto
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BuildingNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Index")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Room")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("DiscountPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ErrorLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StackTrace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("ErrorLogs");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Label", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.MovementHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DestinationZonePositionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("MovementDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SourceZonePositionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DestinationZonePositionId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SourceZonePositionId");

                    b.ToTable("MovementHistories");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("DiscountPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ManufacturerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UnitOfMeasure")
                        .HasColumnType("int");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PreviousCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PreviousCategoryId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ProductInZonePosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ManufactureDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ZonePositionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ZonePositionId");

                    b.ToTable("ProductInZonePositions");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ProductPhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("PhotoData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductPhotos");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Receipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BatchNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReceiptDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShipmentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.HasIndex("UserId");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ReceiptItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ReceiptId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ReceiptId");

                    b.ToTable("ReceiptItems");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReportType")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Shipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BatchNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ShipmentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShipmentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("UserId");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ShipmentItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ShipmentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShipmentId");

                    b.ToTable("ShipmentItems");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Zone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("int");

                    b.Property<int>("ZoneCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WarehouseId");

                    b.HasIndex("ZoneCategoryId");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ZoneCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PreviousCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PreviousCategoryId");

                    b.ToTable("ZoneCategories");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ZonePosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ZoneId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ZoneId");

                    b.ToTable("ZonePositions");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Customer", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Address", "Address")
                        .WithMany("Customers")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Label", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Product", "Product")
                        .WithMany("Labels")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Manufacturer", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Address", "Address")
                        .WithMany("Manufacturers")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Address");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.MovementHistory", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.ZonePosition", "DestinationZonePosition")
                        .WithMany("DestinationMovementHistories")
                        .HasForeignKey("DestinationZonePositionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WarehouseManagementSystem.Models.Entities.Product", "Product")
                        .WithMany("MovementHistories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WarehouseManagementSystem.Models.Entities.ZonePosition", "SourceZonePosition")
                        .WithMany("SourceMovementHistories")
                        .HasForeignKey("SourceZonePositionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DestinationZonePosition");

                    b.Navigation("Product");

                    b.Navigation("SourceZonePosition");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Product", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.ProductCategory", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WarehouseManagementSystem.Models.Entities.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId");

                    b.HasOne("WarehouseManagementSystem.Models.Entities.Warehouse", "Warehouse")
                        .WithMany("Products")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Manufacturer");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ProductCategory", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.ProductCategory", "PreviousCategory")
                        .WithMany()
                        .HasForeignKey("PreviousCategoryId");

                    b.Navigation("PreviousCategory");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ProductInZonePosition", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Product", "Product")
                        .WithMany("ProductsInZonePositions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WarehouseManagementSystem.Models.Entities.ZonePosition", "ZonePosition")
                        .WithMany("ProductsInZonePosition")
                        .HasForeignKey("ZonePositionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("ZonePosition");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ProductPhoto", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Product", "Product")
                        .WithMany("ProductPhotos")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Receipt", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Supplier", "Supplier")
                        .WithMany("Receipts")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WarehouseManagementSystem.Models.Entities.User", "User")
                        .WithMany("Receipts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Supplier");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ReceiptItem", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Product", "Product")
                        .WithMany("ReceiptItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WarehouseManagementSystem.Models.Entities.Receipt", "Receipt")
                        .WithMany("ReceiptItems")
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Report", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.User", "User")
                        .WithMany("Reports")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Shipment", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Customer", "Customer")
                        .WithMany("Shipments")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WarehouseManagementSystem.Models.Entities.User", "User")
                        .WithMany("Shipments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ShipmentItem", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Product", "Product")
                        .WithMany("ShipmentItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WarehouseManagementSystem.Models.Entities.Shipment", "Shipment")
                        .WithMany("ShipmentItems")
                        .HasForeignKey("ShipmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Shipment");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Supplier", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Address", "Address")
                        .WithMany("Customers")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Warehouse", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Address", "Address")
                        .WithMany("Warehouses")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Zone", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Warehouse", "Warehouse")
                        .WithMany("Zones")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WarehouseManagementSystem.Models.Entities.ZoneCategory", "ZoneCategory")
                        .WithMany("Zones")
                        .HasForeignKey("ZoneCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Warehouse");

                    b.Navigation("ZoneCategory");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ZoneCategory", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.ZoneCategory", "PreviousCategory")
                        .WithMany()
                        .HasForeignKey("PreviousCategoryId");

                    b.Navigation("PreviousCategory");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ZonePosition", b =>
                {
                    b.HasOne("WarehouseManagementSystem.Models.Entities.Zone", "Zone")
                        .WithMany("ZonePositions")
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Zone");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Address", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("Manufacturers");

                    b.Navigation("Customers");

                    b.Navigation("Warehouses");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Customer", b =>
                {
                    b.Navigation("Shipments");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Product", b =>
                {
                    b.Navigation("Labels");

                    b.Navigation("MovementHistories");

                    b.Navigation("ProductPhotos");

                    b.Navigation("ProductsInZonePositions");

                    b.Navigation("ReceiptItems");

                    b.Navigation("ShipmentItems");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ProductCategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Receipt", b =>
                {
                    b.Navigation("ReceiptItems");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Shipment", b =>
                {
                    b.Navigation("ShipmentItems");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Supplier", b =>
                {
                    b.Navigation("Receipts");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.User", b =>
                {
                    b.Navigation("Receipts");

                    b.Navigation("Reports");

                    b.Navigation("Shipments");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Warehouse", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("Zones");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.Zone", b =>
                {
                    b.Navigation("ZonePositions");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ZoneCategory", b =>
                {
                    b.Navigation("Zones");
                });

            modelBuilder.Entity("WarehouseManagementSystem.Models.Entities.ZonePosition", b =>
                {
                    b.Navigation("DestinationMovementHistories");

                    b.Navigation("ProductsInZonePosition");

                    b.Navigation("SourceMovementHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
