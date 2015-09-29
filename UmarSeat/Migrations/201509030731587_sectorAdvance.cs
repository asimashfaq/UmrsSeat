namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sectorAdvance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sectors", "country", c => c.String());
            AddColumn("dbo.Sectors", "airline", c => c.String());
            AddColumn("dbo.Sectors", "category", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sectors", "category");
            DropColumn("dbo.Sectors", "airline");
            DropColumn("dbo.Sectors", "country");
        }
    }
}
