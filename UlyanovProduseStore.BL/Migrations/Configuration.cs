namespace UlyanovProduseStore.BL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Model.UPSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true; //TODO: выключать в релизных версиях.
        }

        protected override void Seed(Model.UPSContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
