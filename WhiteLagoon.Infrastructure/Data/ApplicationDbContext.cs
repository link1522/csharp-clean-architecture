using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
            new Villa {
                Id = 1,
                Name = "Royal Villa",
                Description = "lorem ipsum",
                ImageUrl = "https://placehold.co/600x400",
                Occupancy = 4,
                Price = 200,
                Sqft = 550,
            },
            new Villa {
                Id = 2,
                Name = "Pool Villa",
                Description = "lorem ipsum",
                ImageUrl = "https://placehold.co/600x400",
                Occupancy = 4,
                Price = 300,
                Sqft = 550,
            },
            new Villa {
                Id = 3,
                Name = "Luxury Villa",
                Description = "lorem ipsum",
                ImageUrl = "https://placehold.co/600x400",
                Occupancy = 4,
                Price = 400,
                Sqft = 700,
            });
        }

    }
}
