namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockcosttoint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StockTransfers", "cost", c => c.Int(nullable: false));
            AlterColumn("dbo.StockTransfers", "margin", c => c.Int(nullable: false));
            AlterColumn("dbo.StockTransfers", "sellingPrice", c => c.Int(nullable: false));
            AlterColumn("dbo.StockTransfers", "advanceAmount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockTransfers", "advanceAmount", c => c.Single(nullable: false));
            AlterColumn("dbo.StockTransfers", "sellingPrice", c => c.Single(nullable: false));
            AlterColumn("dbo.StockTransfers", "margin", c => c.Single(nullable: false));
            AlterColumn("dbo.StockTransfers", "cost", c => c.Single(nullable: false));
        }
    }
}
