namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pnrStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeatConfirmations", "pnrStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SeatConfirmations", "pnrStatus");
        }
    }
}
