namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredlogout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "requiredLogout", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "requiredLogout");
        }
    }
}
