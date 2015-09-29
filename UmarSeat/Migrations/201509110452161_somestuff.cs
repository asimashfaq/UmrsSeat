namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somestuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agents", "CompanyName", c => c.String());
            AddColumn("dbo.Persons", "branchName", c => c.String());
            AddColumn("dbo.StockTransfers", "isTickted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockTransfers", "isTickted");
            DropColumn("dbo.Persons", "branchName");
            DropColumn("dbo.Agents", "CompanyName");
        }
    }
}
