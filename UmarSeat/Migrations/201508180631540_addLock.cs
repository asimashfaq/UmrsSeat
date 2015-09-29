namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Locked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Locked");
        }
    }
}
