namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pnrLock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.pnrLogs", "pnrLock", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.pnrLogs", "pnrLock");
        }
    }
}
