namespace UlyanovProduseStore.BL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Default : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Name = c.String(),
                    PasswordOrSecondName = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Employees",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Name = c.String(),
                    PasswordOrSecondName = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Products",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Products");
            DropTable("dbo.Employees");
            DropTable("dbo.Clients");
        }
    }
}
