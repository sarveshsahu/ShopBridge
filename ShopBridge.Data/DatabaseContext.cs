using System;
using Microsoft.EntityFrameworkCore;
using ShopBridge.Model;


namespace ShopBridge.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }


        public DbSet<Inventory> Inventory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
