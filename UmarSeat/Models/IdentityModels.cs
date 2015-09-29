using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace UmarSeat.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public int id_SubscriptionPlan { get; set; }
        public int id_Subscription { get; set; }
        public AccountStatus AccountStatus { get; set; }

        public string userRole { get; set; }

        public bool? requiredLogout { get; set; }

        public virtual ICollection<Subscription> Subscription { get; set; }
        public virtual ICollection<person> PersonInfo { get; set; }
    }

    public enum AccountStatus
    {
        Active,
        Blocked,
        Deleted,
        Pending
    }
    public class Client
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Secret { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public ApplicationTypes ApplicationType { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        [MaxLength(100)]
        public string AllowedOrigin { get; set; }
    }
    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    };
    public class RefreshToken
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Subject { get; set; }
        [Required]
        [MaxLength(50)]
        public string ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        [Required]
        public string ProtectedTicket { get; set; }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
           
        }
        public DbSet<subscriptionPlan> SubscriptionPlan { get; set; }
        public DbSet<person> Persons { get; set;}
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<airLine> Airline { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<branches> Branch { get; set; }
        public DbSet<Sector> Sector { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SeatConfirmation> SeatConfirmation { get; set; }
        public DbSet<Agents> Agent { get; set; }
        public DbSet<StockTransfer> StockTransfer { get; set; }
        public DbSet<UserRoles> UserRole { get; set; }

        public DbSet<Stock> Stock { get; set; }
        public DbSet<pnrLog> pnrLogs { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public System.Data.Entity.DbSet<UmarSeat.Models.Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<pnrLog>().Property(s => s.RowVersion).IsRowVersion();
            modelBuilder.Entity<pnrLog>().Property(p => p.RowVersion).IsConcurrencyToken();

            base.OnModelCreating(modelBuilder);
        }
    }
}