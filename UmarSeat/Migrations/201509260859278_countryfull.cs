namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class countryfull : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cities", "countryFull", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cities", "countryFull");
        }
    }
}
