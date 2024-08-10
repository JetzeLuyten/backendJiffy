using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using JiffyBackend.DAL.Entity;

namespace JiffyBackend.DAL
{
    public class JiffyDbContext : DbContext
    {
        public JiffyDbContext()
        {

        }

        public JiffyDbContext(DbContextOptions<JiffyDbContext> options) : base(options)
        {
        }
        public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceType>()
               .HasMany(st => st.Services)
               .WithOne(st => st.ServiceType)
               .HasForeignKey(st => st.ServiceTypeId)
               .IsRequired();

            modelBuilder.Entity<Service>()
                .HasMany(s => s.Bookings)
                .WithOne(b => b.Service)
                .HasForeignKey(b => b.ServiceId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Services)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
               .HasOne(b => b.Booker)
               .WithMany(u => u.Bookings) // Assuming a User can have multiple Bookings
               .HasForeignKey(b => b.BookerId)
               .OnDelete(DeleteBehavior.Restrict) // Specify ON DELETE RESTRICT to avoid cycles or multiple cascade paths
               .IsRequired();

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Service)
                .WithMany(o => o.Bookings) // Assuming an Offer can have multiple Bookings
                .HasForeignKey(b => b.ServiceId)
                .OnDelete(DeleteBehavior.Restrict) // Specify ON DELETE RESTRICT to avoid cycles or multiple cascade paths
                .IsRequired();

            //Other side
            //modelBuilder.Entity<Activity>()
            //    .HasOne(e => e.Trip)
            //    .WithMany(e => e.Activities)
            //    .HasForeignKey(e => e.TripId)
            //    .IsRequired();

            modelBuilder.Entity<ServiceType>().ToTable("ServiceType");
            modelBuilder.Entity<Service>().ToTable("Service");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Booking>().ToTable("Booking");
        }
    }
}