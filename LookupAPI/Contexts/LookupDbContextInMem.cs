using LookupAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace LookupAPI.Contexts
{
    public class LookupDbContextInMem: IdentityDbContext<ApplicationUser>
    {
        public LookupDbContextInMem(DbContextOptions<LookupDbContextInMem> options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //will be passed to mssql db in next versions
            optionsBuilder.UseInMemoryDatabase(databaseName: "LookupDb");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
              .Property(e => e.FirstName)
            .HasMaxLength(100);

            builder.Entity<ApplicationUser>()
                .Property(e => e.LastName)
            .HasMaxLength(100);

            builder.Entity<Film>()
                 .HasOne(c => c.ApplicationUser)
                .WithMany(u => u.Films)
                .HasForeignKey(c => c.ApplicationUserId)
                .IsRequired();

            builder.Entity<Game>()
                 .HasOne(c => c.ApplicationUser)
                .WithMany(u => u.Games)
                .HasForeignKey(c => c.ApplicationUserId)
                .IsRequired();

            builder.Entity<Recipe>()
                 .HasOne(c => c.ApplicationUser)
                .WithMany(u => u.Recipes)
                .HasForeignKey(c => c.ApplicationUserId)
                .IsRequired();

        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}
