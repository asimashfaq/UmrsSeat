namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class branchCountryNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.branches", "branchCountry", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.branches", "branchCountry", c => c.String(nullable: false));
        }
    }
}
