namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeatConfirmationCountry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeatConfirmations", "airLine", c => c.String());
            AddColumn("dbo.SeatConfirmations", "country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SeatConfirmations", "country");
            DropColumn("dbo.SeatConfirmations", "airLine");
        }
    }
}
