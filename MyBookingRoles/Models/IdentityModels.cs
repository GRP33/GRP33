using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyBookingRoles.Models.Booking;

namespace MyBookingRoles.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        //public virtual ICollection<IdentityRole> collection { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            base.Configuration.ProxyCreationEnabled = false;

            //Database.SetInitializer(new DropCreateDatabaseAlways<ApplicationDbContext>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //Bookings

        public DbSet<Administration> Administrations { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Bookings> Bookings { get; set; }

        //public virtual DbSet<CustomerProfile> Customers { get; set; }

       // public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<Service> Services { get; set; }

        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<BookPayment> Payments { get; set; }

        //Store Internal
        public System.Data.Entity.DbSet<MyBookingRoles.Models.Store.Category> Category { get; set; }
        public System.Data.Entity.DbSet<MyBookingRoles.Models.Store.Brand> Brands { get; set; }
        public System.Data.Entity.DbSet<MyBookingRoles.Models.Store.Discount> Discounts { get; set; }
        public System.Data.Entity.DbSet<MyBookingRoles.Models.Store.Product> Products { get; set; }
        public System.Data.Entity.DbSet<MyBookingRoles.Models.Store.Order> Orders { get; set; }
        public System.Data.Entity.DbSet<MyBookingRoles.Models.Store.OrderDetails> OrderDetails { get; set; }
        //public System.Data.Entity.DbSet<MyBookingRoles.Models.Store.Item> Items { get; set; }

        //Rate_Service
        public System.Data.Entity.DbSet<MyBookingRoles.Models.RateService.Rates> Rates { get; set; }
        public System.Data.Entity.DbSet<MyBookingRoles.Models.RateService.Rate_Service> Rate_Services { get; set; }

    }
}