namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class branchCountry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.branches", "branchCountry", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.branches", "branchCountry");
        }
    }
}
