namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StockTransfer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockTransfers",
                c => new
                    {
                        id_StockTransfer = c.Int(nullable: false, identity: true),
                        country = c.String(),
                        pnrNumber = c.String(),
                        airLine = c.String(),
                        stockId = c.String(),
                        idAgent = c.String(),
                        transferingBranch = c.String(),
                        recevingBranch = c.String(),
                        sellingBranch = c.String(),
                        cost = c.Single(nullable: true),
                        margin = c.Single(nullable: true),
                        sellingPrice = c.Single(nullable: true),
                        noOfSeats = c.Int(nullable: true),
                        isPackage = c.Boolean(nullable: true),
                        advanceAmount = c.Single(nullable: true),
                        advanceDate = c.DateTime(nullable: true),
                        gdsPnrNumber = c.String(),
                        catalystInvoiceNumber = c.String(),
                        createAt = c.DateTime(nullable: true),
                        UpdateAt = c.DateTime(nullable: true),
                        id_Subscription = c.Int(nullable: true),
                        isDelete = c.Boolean(nullable: true),
                    })
                .PrimaryKey(t => t.id_StockTransfer);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StockTransfers");
        }
    }
}
