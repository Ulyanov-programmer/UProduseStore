using System;
using System.Data.Entity;

namespace UlyanovProduseStore.BL.Model
{
    public class UPSEmployeeContext : DbContext
    {
        public UPSEmployeeContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }

        public const string StringConnectToEmployeeServer = "MainEmployeeServer";
        public const string StringConnectToTestEmployeeServr = "MainEmployeeServer";
    }
}
