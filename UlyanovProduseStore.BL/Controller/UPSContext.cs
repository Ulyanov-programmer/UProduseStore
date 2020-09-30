using System;
using System.Data.Entity;

namespace UlyanovProduseStore.BL.Model
{
    public class UPSContext : DbContext
    {
        public UPSContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }

        public UPSContext()
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }

        #region const

        /// <summary>
        /// Константа строки подключения к базовому серверу.
        /// </summary>
        public const string StringConnectToMainServer = "MainDataBase";


        //TODO: Не работает возможность создания тестовой базы данных.
        ///// <summary>
        ///// Константа строки подключения к тестовому серверу.
        ///// </summary>
        //public const string StringConnectToTestServer = "UPSTestDataBase";

        #endregion
    }
}
