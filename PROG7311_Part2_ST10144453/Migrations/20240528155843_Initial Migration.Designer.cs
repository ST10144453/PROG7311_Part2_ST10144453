﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PROG7311_Part2_ST10144453.Data;

#nullable disable

namespace PROG7311_Part2_ST10144453.Migrations
{
    [DbContext(typeof(Part2DbContext))]
    [Migration("20240528155843_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PROG7311_Part2_ST10144453.Models.Domain.Farmer", b =>
                {
                    b.Property<Guid>("FarmerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("farmer_id");

                    b.Property<string>("About")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("about");

                    b.Property<string>("FarmName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("farm_name");

                    b.Property<string>("Specialty")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("specialty");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.HasKey("FarmerId");

                    b.HasIndex("UserID");

                    b.ToTable("Farmers");
                });

            modelBuilder.Entity("PROG7311_Part2_ST10144453.Models.Domain.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("product_id");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("category");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<Guid>("FarmerId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("farmer_id");

                    b.Property<float>("Price")
                        .HasColumnType("real")
                        .HasColumnName("price");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("product_name");

                    b.Property<DateTime>("productionDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("production_date");

                    b.HasKey("ProductId");

                    b.HasIndex("FarmerId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PROG7311_Part2_ST10144453.Models.Domain.ProductPhoto", b =>
                {
                    b.Property<Guid>("ProductPhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("product_photo_id");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("photo");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("product_id");

                    b.HasKey("ProductPhotoId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductPhotos");
                });

            modelBuilder.Entity("PROG7311_Part2_ST10144453.Models.Domain.Staff", b =>
                {
                    b.Property<Guid>("StaffId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("staff_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.HasKey("StaffId");

                    b.HasIndex("UserId");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("PROG7311_Part2_ST10144453.Models.Domain.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("account_type");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<string>("ProfilePhoto")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("profile_photo");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("surname");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PROG7311_Part2_ST10144453.Models.Domain.Farmer", b =>
                {
                    b.HasOne("PROG7311_Part2_ST10144453.Models.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PROG7311_Part2_ST10144453.Models.Domain.Product", b =>
                {
                    b.HasOne("PROG7311_Part2_ST10144453.Models.Domain.Farmer", "Farmer")
                        .WithMany()
                        .HasForeignKey("FarmerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farmer");
                });

            modelBuilder.Entity("PROG7311_Part2_ST10144453.Models.Domain.ProductPhoto", b =>
                {
                    b.HasOne("PROG7311_Part2_ST10144453.Models.Domain.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PROG7311_Part2_ST10144453.Models.Domain.Staff", b =>
                {
                    b.HasOne("PROG7311_Part2_ST10144453.Models.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}