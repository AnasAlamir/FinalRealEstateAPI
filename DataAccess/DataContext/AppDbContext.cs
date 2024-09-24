using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataContext
{
    internal class AppDbContext : DbContext
    {
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyStatus> propertyStatuses { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Amenities> Amenities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-5JC2MA4\\SQLEXPRESS;Initial Catalog=FinalRealEstateDb;" +
                "Integrated Security=True;Trust Server Certificate=True;" +
                "MultipleActiveResultSets=True;");///MultipleActiveResultSets
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Restrict delete on UserId to avoid multiple cascade paths
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict on UserId

            // Cascade delete on PropertyId to remove Favorites when a Property is deleted
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Property)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade on PropertyId

            // Repeat similar logic for Inquiries if needed
            modelBuilder.Entity<Inquiry>()
                .HasOne(i => i.User)
                .WithMany(u => u.Inquiries)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict on UserId

            modelBuilder.Entity<Inquiry>()
                .HasOne(i => i.Property)
                .WithMany(p => p.Inquiries)
                .HasForeignKey(i => i.PropertyId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade on PropertyId

            base.OnModelCreating(modelBuilder);

            //start
            List<Amenities> amenities = new List<Amenities>();
            for (int i = 1; i <= Math.Pow(2, 10); i++)
            {
                Amenities amenity = new Amenities();
                int number = i - 1;
                string binaryString = Convert.ToString(number, 2);
                string binaryStringPaded = binaryString.PadLeft(10, '0');
                //1 = 00-0000-0001
                amenity.Id = i;
                amenity.HasGarage = binaryStringPaded[9] == '1';
                amenity.Two_Stories = binaryStringPaded[8] == '1';
                amenity.Laundry_Room = binaryStringPaded[7] == '1';
                amenity.HasPool = binaryStringPaded[6] == '1';
                amenity.HasGarden = binaryStringPaded[5] == '1';
                amenity.HasElevator = binaryStringPaded[4] == '1';
                amenity.HasBalcony = binaryStringPaded[3] == '1';
                amenity.HasParking = binaryStringPaded[2] == '1';
                amenity.HasCentralHeating = binaryStringPaded[1] == '1';
                amenity.IsFurnished = binaryStringPaded[0] == '1';

                amenities.Add(amenity);
            }
            //end

            modelBuilder.Entity<Amenities>().HasData(amenities);

            modelBuilder.Entity<PropertyType>().HasData(new List<PropertyType> {
            new PropertyType { Id = 1, Type = "Apartment" },
            new PropertyType { Id = 2, Type = "House" },
            new PropertyType { Id = 3, Type = "Villa" },
            new PropertyType { Id = 4, Type = "Townhouse" },
            new PropertyType { Id = 5, Type = "Commercial" },
            new PropertyType { Id = 6, Type = "Land" }, 
            new PropertyType { Id = 7, Type = "Duplex" },
            new PropertyType { Id = 8, Type = "Studio" }
            });

            modelBuilder.Entity<PropertyStatus>().HasData(new List<PropertyStatus> {
            new PropertyStatus { Id = 1, Status = "For Sale"},
            new PropertyStatus { Id = 2, Status = "For Rent" },
            new PropertyStatus { Id = 3, Status = "Sold" },
            new PropertyStatus { Id = 4, Status = "Rented" },
            new PropertyStatus { Id = 5, Status = "Under Offer" },
            new PropertyStatus { Id = 6, Status = "Pending" },
            new PropertyStatus { Id = 7, Status = "Withdrawn" },
            new PropertyStatus { Id = 8, Status = "Available" }
        });

            modelBuilder.Entity<City>().HasData(new List<City>
    {
        new City { Id = 1, CityName = "Cairo" },
        new City { Id = 2, CityName = "New York" },
        new City { Id = 3, CityName = "London" },
        new City { Id = 4, CityName = "Tokyo" },
        new City { Id = 5, CityName = "Paris" },
        new City { Id = 6, CityName = "Berlin" },
        new City { Id = 7, CityName = "Sydney" },
        new City { Id = 8, CityName = "Dubai" },
        new City { Id = 9, CityName = "Rio de Janeiro" },
        new City { Id = 10, CityName = "Mumbai" },
        new City { Id = 11, CityName = "Istanbul" },
        new City { Id = 12, CityName = "Moscow" },
        new City { Id = 13, CityName = "Mexico City" }
    });

            ///handel in service

            modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(user => user.PhoneNumber )
            .IsUnique();

            modelBuilder.Entity<User>()
                .Property(user => user.FullName)
                .HasComputedColumnSql("[F_Name] + ' ' + [L_Name]");

            modelBuilder.Entity<Property>()
            .HasIndex(property => new { property.Name, property.Address, property.UserId })
            .IsUnique();

            modelBuilder.Entity<Inquiry>()
            .HasIndex(inquiry => new { inquiry.UserId,inquiry.PropertyId })
            .IsUnique();
            modelBuilder.Entity<Favorite>()
            .HasIndex(inquiry => new { inquiry.UserId, inquiry.PropertyId })
            .IsUnique();

            modelBuilder.Entity<PropertyImage>()
            .HasIndex(pi => pi.Path)
            .IsUnique();
        }
    }
}
