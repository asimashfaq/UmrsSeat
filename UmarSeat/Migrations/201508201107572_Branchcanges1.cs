namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Branchcanges1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.branches", "branchCity", c => c.String());
            AlterColumn("dbo.branches", "branchAddress", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.branches", "branchAddress", c => c.String(nullable: false));
            AlterColumn("dbo.branches", "branchCity", c => c.String(nullable: false));
        }
    }
}
