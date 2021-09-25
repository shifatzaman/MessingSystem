using MessingSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<AppDbContext>>()))
            {
                if (!context.InventoryItemTypes.Any())
                {
                    context.InventoryItemTypes.AddRange(

                            new InventoryItemType
                            {
                                Name = "Rice",
                                Unit = "Kg",
                                UnitPrice = 30

                            },

                            new InventoryItemType
                            {
                                Name = "Pulse",
                                Unit = "Kg",
                                UnitPrice = 20

                            },

                            new InventoryItemType
                            {
                                Name = "Chicken",
                                Unit = "Kg",
                                UnitPrice = 100
                            },

                            new InventoryItemType
                            {
                                Name = "Vegetable",
                                Unit = "Kg",
                                UnitPrice = 20
                            }
                        );

                    context.SaveChanges();
                }


                // Look for any movies.
                if (! context.Users.Any())
                {
                    context.Users.AddRange(
                    GetAdminUser(context)
                );

                    context.SaveChanges();
                }

                

                

                
            }
        }

        private static User GetAdminUser(AppDbContext context)
        {
            var user = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@ms.com"
            };

            byte[] passwordHash, passwordSalt;
            CommonUtilities.CreatePasswordHash("123456", out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            return user;

        }
    }
}
