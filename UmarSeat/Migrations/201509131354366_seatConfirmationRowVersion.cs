namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seatConfirmationRowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeatConfirmations", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SeatConfirmations", "RowVersion");
        }
    }
}
