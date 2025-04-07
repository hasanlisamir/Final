using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using DataAccess.Dtos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Data

{
    public class CarMSAppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public CarMSAppDbContext(DbContextOptions<CarMSAppDbContext> dbContextOption) : base(dbContextOption)
        {

        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CarModel> Models { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<AppRole> Roles { get; set; }


        internal void Find(int id)
        {
            throw new NotImplementedException();
        }

        internal void Update()
        {
            throw new NotImplementedException();
        }
    }
}
