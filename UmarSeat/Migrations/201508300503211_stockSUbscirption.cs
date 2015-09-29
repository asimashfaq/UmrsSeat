namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockSUbscirption : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "id_Subscription", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "id_Subscription");
        }
    }
}
