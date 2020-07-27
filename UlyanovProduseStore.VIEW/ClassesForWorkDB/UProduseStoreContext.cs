using System;
using System.Data.Entity;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.BL
{
    class UProduseStoreContext : DbContext
    {
        public UProduseStoreContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
        
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
