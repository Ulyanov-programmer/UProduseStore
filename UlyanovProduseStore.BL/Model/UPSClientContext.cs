using System;
using System.Data.Entity;

namespace UlyanovProduseStore.BL.Model
{
    public class UPSClientContext : DbContext
    {
        public UPSClientContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public const string StringConnectToMainClientServer = "MainClientServer";
        public const string StringConnectToTestMainClientServer = "TestClientServer";
    }
}
