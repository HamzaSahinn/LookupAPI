using LookupAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace LookupAPI.Contexts
{
    public class LookupDbContextInMem:DbContext
    {
        public LookupDbContextInMem(DbContextOptions<LookupDbContextInMem> options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "LookupDb");
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
    }
}
