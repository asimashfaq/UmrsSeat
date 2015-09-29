namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cities", "Name", c => c.String());
            DropColumn("dbo.Cities", "cityName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cities", "cityName", c => c.String());
            DropColumn("dbo.Cities", "Name");
        }
    }
}
