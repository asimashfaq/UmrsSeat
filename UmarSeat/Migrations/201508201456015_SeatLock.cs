namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeatLock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeatConfirmations", "Locked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SeatConfirmations", "Locked");
        }
    }
}
