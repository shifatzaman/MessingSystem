﻿// <auto-generated />
using System;
using MessingSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MessingSystem.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210925111028_DbUpdate252105")]
    partial class DbUpdate252105
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MessingSystem.Domain.InventoryItem", b =>
                {
                    b.Property<int>("InventoryItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("date");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("isdeleted");

                    b.Property<int>("ItemType")
                        .HasColumnType("int")
                        .HasColumnName("itemtype");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("quantity");

                    b.HasKey("InventoryItemId");

                    b.ToTable("inventoryitems");
                });

            modelBuilder.Entity("MessingSystem.Domain.InventoryItemType", b =>
                {
                    b.Property<int>("ItemTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Quantity");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("unit");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("unitprice");

                    b.HasKey("ItemTypeId");

                    b.ToTable("inventoryitemtypes");
                });

            modelBuilder.Entity("MessingSystem.Domain.Meal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("MessingSystem.Domain.MealItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ItemType")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("MealItems");
                });

            modelBuilder.Entity("MessingSystem.Domain.MemberMeal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("BreakFastEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("DinnerEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("LunchEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<bool>("TeaBreakEnabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("MemberMeals");
                });

            modelBuilder.Entity("MessingSystem.Domain.MessMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Appt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bnumb")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfEntry")
                        .HasColumnType("datetime2");

                    b.Property<string>("MaritialStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MemberStatus")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rank")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MessMembers");
                });

            modelBuilder.Entity("MessingSystem.Domain.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fname");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("isdeleted");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("lname");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("passwordhash");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("passwordsalt");

                    b.HasKey("UserId");

                    b.ToTable("users");
                });
#pragma warning restore 612, 618
        }
    }
}
