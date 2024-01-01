﻿// <auto-generated />
using System;
using CircuitsUc.InfraStructure.Presistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CircuitsUc.InfraStructure.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20240101224044_Create-Document")]
    partial class CreateDocument
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CircuitsUc.Domain.Entities.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileExtension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMain")
                        .HasColumnType("bit");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rank")
                        .HasColumnType("int");

                    b.Property<string>("RefEntityID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefEntityTypeID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("CircuitsUc.Domain.Entities.SecurityUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AlternativePhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("Code")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PreferedContact")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("SecurityUsers");
                });

            modelBuilder.Entity("CircuitsUc.Domain.Entities.Document", b =>
                {
                    b.HasOne("CircuitsUc.Domain.Entities.SecurityUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CircuitsUc.Domain.Entities.SecurityUser", "UpdatedByUser")
                        .WithMany()
                        .HasForeignKey("UpdatedBy");

                    b.Navigation("CreatedByUser");

                    b.Navigation("UpdatedByUser");
                });

            modelBuilder.Entity("CircuitsUc.Domain.Entities.SecurityUser", b =>
                {
                    b.HasOne("CircuitsUc.Domain.Entities.SecurityUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CircuitsUc.Domain.Entities.SecurityUser", "UpdatedByUser")
                        .WithMany()
                        .HasForeignKey("UpdatedBy");

                    b.Navigation("CreatedByUser");

                    b.Navigation("UpdatedByUser");
                });
#pragma warning restore 612, 618
        }
    }
}
