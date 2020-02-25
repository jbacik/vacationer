using System.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class VacationContext : DbContext
    {
        public VacationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set;}
        public DbSet<VacationPool> VacationPools { get; set; }
    }
}