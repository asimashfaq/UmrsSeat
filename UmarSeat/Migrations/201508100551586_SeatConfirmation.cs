namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeatConfirmation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeatConfirmations",
                c => new
                    {
                        id_SeatConfirmation = c.Int(nullable: false, identity: true),
                        pnrNumber = c.String(),
                        newPnrNumber = c.String(),
                        stockId = c.String(),
                        outBoundDate = c.DateTime(nullable: false),
                        inBoundDate = c.DateTime(nullable: false),
                        outBoundSector = c.String(),
                        inBoundSector = c.String(),
                        noOfSeats = c.Int(nullable: false),
                        cost = c.Int(nullable: false),
                        category = c.String(),
                        recevingBranch = c.String(),
                        emdNumber = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        id_Subscription = c.Int(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        timeLimit = c.DateTime(nullable: false),
                        isDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_SeatConfirmation);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SeatConfirmations");
        }
    }
}
