using System;
using System.Data.Entity.Infrastructure;
using UlyanovProduseStore.BL;

namespace UlyanovProduseStore.VIEW
{
    internal class MigrationsContextFactory : IDbContextFactory<UProduseStoreContext>
    {
        public UProduseStoreContext Create()
        {
            return new UProduseStoreContext("connectionStringName");
        }
    }
}
