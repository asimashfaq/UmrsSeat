namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cities", "city", c => c.String());
            AddColumn("dbo.Cities", "country", c => c.String());
            DropColumn("dbo.Cities", "Name");
            DropColumn("dbo.Cities", "CountryCode");
            DropColumn("dbo.Cities", "District");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cities", "District", c => c.String());
            AddColumn("dbo.Cities", "CountryCode", c => c.String());
            AddColumn("dbo.Cities", "Name", c => c.String());
            DropColumn("dbo.Cities", "country");
            DropColumn("dbo.Cities", "city");
        }
    }
}
