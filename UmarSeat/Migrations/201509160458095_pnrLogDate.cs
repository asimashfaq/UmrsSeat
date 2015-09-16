namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pnrLogDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.pnrLogs", "createdAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.pnrLogs", "createdAt");
        }
    }
}
