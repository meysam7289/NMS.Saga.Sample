using Microsoft.EntityFrameworkCore;
using NMS.Saga.Sample.Contracts.Models;

namespace NMS.Saga.Sample.Core.DataLayer
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        public DbSet<Coding> Coding { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coding>().Property(t => t.IdCoding).ValueGeneratedNever();
            base.OnModelCreating(modelBuilder);
        }
    }
}
