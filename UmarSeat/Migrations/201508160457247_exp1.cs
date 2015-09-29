namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exp1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StockTransfers", "advanceDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StockTransfers", "advanceDate", c => c.DateTime(nullable: false));
        }
    }
}
