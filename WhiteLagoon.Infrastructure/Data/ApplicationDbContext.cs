using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<Amenity> Amenities { get; set; }

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

            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    Villa_Number = 101,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    Villa_Number = 102,
                    VillaId = 2,
                },
                new VillaNumber
                {
                    Villa_Number = 201,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    Villa_Number = 202,
                    VillaId = 2,
                },
                new VillaNumber
                {
                    Villa_Number = 203,
                    VillaId = 3,
                }
            );


            modelBuilder.Entity<Amenity>().HasData(
                new Amenity {
                    Id = 1,
                    Name = "Swimming Pool",
                    VillaId = 1
                },
                new Amenity {
                    Id = 2,
                    Name = "Spa",
                    VillaId = 1
                },
                new Amenity {
                    Id = 3,
                    Name = "Gym",
                    VillaId = 2 
                },
                new Amenity {
                    Id = 4,
                    Name = "Restaurant",
                    VillaId = 2 
                },
                new Amenity {
                    Id = 5,
                    Name = "Bar",
                    VillaId = 3
                }
            );
        }

    }
}
