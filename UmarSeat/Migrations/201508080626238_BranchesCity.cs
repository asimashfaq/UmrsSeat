namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BranchesCity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.branches",
                c => new
                    {
                        id_branch = c.Int(nullable: false, identity: true),
                        branchName = c.String(nullable: false),
                        branchCitry = c.String(nullable: false),
                        id_Subscription = c.Int(nullable: false),
                        id_BranchManager = c.Int(nullable: false),
                        branchAddress = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_branch);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        id_City = c.Int(nullable: false, identity: true),
                        cityName = c.String(),
                        CountryCode = c.String(),
                        District = c.String(),
                    })
                .PrimaryKey(t => t.id_City);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cities");
            DropTable("dbo.branches");
        }
    }
}
