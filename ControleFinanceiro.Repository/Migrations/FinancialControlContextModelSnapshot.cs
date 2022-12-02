﻿// <auto-generated />
using System;
using ControleFinanceiro.Repository.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ControleFinanceiro.Repository.Migrations
{
    [DbContext(typeof(FinancialControlContext))]
    partial class FinancialControlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ControleFinanceiro.Domain.Models.TransactionViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("UserViewModelId")
                        .HasColumnType("int");

                    b.Property<float>("Value")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("UserViewModelId");

                    b.ToTable("TransactionViewModels");
                });

            modelBuilder.Entity("ControleFinanceiro.Domain.Models.UserViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<float>("Balance")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserViewModels");
                });

            modelBuilder.Entity("ControleFinanceiro.Domain.Models.TransactionViewModel", b =>
                {
                    b.HasOne("ControleFinanceiro.Domain.Models.UserViewModel", null)
                        .WithMany("TransactionViewModels")
                        .HasForeignKey("UserViewModelId");
                });

            modelBuilder.Entity("ControleFinanceiro.Domain.Models.UserViewModel", b =>
                {
                    b.Navigation("TransactionViewModels");
                });
#pragma warning restore 612, 618
        }
    }
}