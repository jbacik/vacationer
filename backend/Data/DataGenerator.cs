using System;
using System.Collections.ObjectModel;
using System.Linq;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Data 
{
    public class DataGenerator 
    {
        public static void Initialize (IServiceProvider serviceProvider)
        {
            using (var dbContext = new VacationContext(
                serviceProvider.GetRequiredService<DbContextOptions<VacationContext>>()))
            {
                if (dbContext.Users.Any())
                {
                    return; //Data already seeded
                }

                var user = new User{
                    Id = 3,
                    FirstName = "Vance",
                    LastName = "Vacations",
                    UserName = "inmemory",
                    IsActive = true
                };

                user.VacationPools = new Collection<VacationPool>();

                user.VacationPools.Add(
                    new VacationPool {
                        StartDate = new DateTime(2019,01,01),
                        Hours = 80,
                        IsActive = false
                    });
                
                user.VacationPools.Add(
                    new VacationPool {
                        StartDate = new DateTime(2020,01,01),
                        Hours = 100,
                        IsActive = true
                    });

                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
        }
    }
}