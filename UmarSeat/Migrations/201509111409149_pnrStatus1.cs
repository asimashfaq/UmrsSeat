namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pnrStatus1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeatConfirmations", "pnrStatus1", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SeatConfirmations", "pnrStatus1");
        }
    }
}
