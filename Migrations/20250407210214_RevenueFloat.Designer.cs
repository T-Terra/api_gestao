﻿// <auto-generated />
using System;
using Expenses.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Expenses.Migrations
{
    [DbContext(typeof(ExpenseContext))]
    [Migration("20250407210214_RevenueFloat")]
    partial class RevenueFloat
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Expenses.Models.ExpensesModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AmountExpense")
                        .HasColumnType("integer");

                    b.Property<string>("CategoryExpense")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateExpense")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DescriptionExpense")
                        .HasColumnType("text");

                    b.Property<string>("NameExpense")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Expenses.Models.RevenueModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<float>("AmountRevenue")
                        .HasColumnType("real");

                    b.Property<DateTime>("DateRevenue")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Revenues");
                });
#pragma warning restore 612, 618
        }
    }
}
