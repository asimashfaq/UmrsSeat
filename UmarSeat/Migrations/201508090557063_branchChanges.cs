namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class branchChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.branches", "branchCity", c => c.String(nullable: false));
            DropColumn("dbo.branches", "branchCitry");
        }
        
        public override void Down()
        {
            AddColumn("dbo.branches", "branchCitry", c => c.String(nullable: false));
            DropColumn("dbo.branches", "branchCity");
        }
    }
}
