namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pnrLogs11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.pnrLogs", "groupSplit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.pnrLogs", "groupSplit");
        }
    }
}
