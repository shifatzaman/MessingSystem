using MessingSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("users");

                e.HasKey(e => e.UserId);

                e.Property(pr => pr.UserId).HasColumnName("id");

                e.Property(pr => pr.FirstName).HasColumnName("fname");

                e.Property(pr => pr.LastName).HasColumnName("lname");

                e.Property(pr => pr.Email).HasColumnName("email");

                e.Property(pr => pr.IsDeleted).HasColumnName("isdeleted");

                e.Property(pr => pr.PasswordHash).HasColumnName("passwordhash");

                e.Property(pr => pr.PasswordSalt).HasColumnName("passwordsalt");

            });


            modelBuilder.Entity<InventoryItem>(e =>
            {
                e.ToTable("inventoryitems");

                e.HasKey(e => e.InventoryItemId);

                e.Property(pr => pr.InventoryItemId).HasColumnName("id");

                e.Property(pr => pr.Date).HasColumnName("date");

                e.Property(pr => pr.ItemType).HasColumnName("itemtype");

                e.Property(pr => pr.Quantity).HasColumnName("quantity");

                e.Property(pr => pr.IsDeleted).HasColumnName("isdeleted");

                e.HasQueryFilter(pr => !pr.IsDeleted);

            });

            modelBuilder.Entity<InventoryItemType>(e =>
            {
                e.ToTable("inventoryitemtypes");

                e.HasKey(e => e.ItemTypeId);

                e.Property(pr => pr.ItemTypeId).HasColumnName("id");

                e.Property(pr => pr.Name).HasColumnName("name");

                e.Property(pr => pr.Unit).HasColumnName("unit");

                e.Property(pr => pr.UnitPrice).HasColumnName("unitprice");

                e.Property(pr => pr.Quantity).HasColumnName("Quantity");

            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryItemType> InventoryItemTypes { get; set; }
        public DbSet<MessMember> MessMembers { get; set; }
        public DbSet<MemberMeal> MemberMeals { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealItem> MealItems { get; set; }
        public DbSet<DailyMessing> DailyMessings { get; set; }
        public DbSet<DailyMessingItem> DailyMessingItems { get; set; }


    }
}
