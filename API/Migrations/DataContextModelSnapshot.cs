﻿// <auto-generated />
using System;
using API.Utility.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("API.Utility.Database.Models.Appointment", b =>
                {
                    b.Property<int>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AppointmentId"));

                    b.Property<int>("BarberEmployeeId")
                        .HasColumnType("integer");

                    b.Property<int>("CustomerUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("AppointmentId");

                    b.HasIndex("BarberEmployeeId");

                    b.HasIndex("CustomerUserId");

                    b.ToTable("Appointments", (string)null);
                });

            modelBuilder.Entity("API.Utility.Database.Models.Authorization", b =>
                {
                    b.Property<int>("AuthorizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AuthorizationId"));

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("AuthorizationId");

                    b.HasIndex("UserId");

                    b.ToTable("Authorization", (string)null);
                });

            modelBuilder.Entity("API.Utility.Database.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EmployeeId"));

                    b.Property<int?>("ManagerEmployeeId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("EmployeeId");

                    b.HasIndex("ManagerEmployeeId");

                    b.HasIndex("UserId");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("API.Utility.Database.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderId"));

                    b.Property<int>("AppointmentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("OrderId");

                    b.HasIndex("AppointmentId");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("API.Utility.Database.Models.Price", b =>
                {
                    b.Property<int>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PriceId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("PriceId");

                    b.HasIndex("ProductId");

                    b.ToTable("Prices", (string)null);
                });

            modelBuilder.Entity("API.Utility.Database.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("OrderId")
                        .HasColumnType("integer");

                    b.HasKey("ProductId");

                    b.HasIndex("OrderId");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("API.Utility.Database.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("character varying(320)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Password")
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.HasKey("UserId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("API.Utility.Database.Models.Appointment", b =>
                {
                    b.HasOne("API.Utility.Database.Models.Employee", "Barber")
                        .WithMany()
                        .HasForeignKey("BarberEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Utility.Database.Models.User", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Barber");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("API.Utility.Database.Models.Authorization", b =>
                {
                    b.HasOne("API.Utility.Database.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Utility.Database.Models.Employee", b =>
                {
                    b.HasOne("API.Utility.Database.Models.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerEmployeeId");

                    b.HasOne("API.Utility.Database.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manager");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Utility.Database.Models.Order", b =>
                {
                    b.HasOne("API.Utility.Database.Models.Appointment", "Appointment")
                        .WithMany()
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");
                });

            modelBuilder.Entity("API.Utility.Database.Models.Price", b =>
                {
                    b.HasOne("API.Utility.Database.Models.Product", null)
                        .WithMany("PriceHistory")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("API.Utility.Database.Models.Product", b =>
                {
                    b.HasOne("API.Utility.Database.Models.Order", null)
                        .WithMany("Products")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("API.Utility.Database.Models.Order", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("API.Utility.Database.Models.Product", b =>
                {
                    b.Navigation("PriceHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
