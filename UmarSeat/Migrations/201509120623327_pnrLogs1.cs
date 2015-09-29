namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pnrLogs1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.pnrLogs",
                c => new
                    {
                        pnrLogId = c.Int(nullable: false, identity: true),
                        pnrNumber = c.String(),
                        totalSeats = c.Int(nullable: false),
                        avaliableSeats = c.Int(nullable: false),
                        sellSeats = c.Int(nullable: false),
                        transferSeats = c.Int(nullable: false),
                        receiveSeats = c.Int(nullable: false),
                        branchName = c.String(),
                        idSubscription = c.Int(nullable: false),
                        pnrStatus = c.String(),
                    })
                .PrimaryKey(t => t.pnrLogId);
            
            AddColumn("dbo.StockTransfers", "pnrStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockTransfers", "pnrStatus");
            DropTable("dbo.pnrLogs");
        }
    }
}
