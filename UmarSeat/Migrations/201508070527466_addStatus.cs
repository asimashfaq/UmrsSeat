namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "id_Subscription", c => c.Int());
            AddColumn("dbo.AspNetUsers", "AccountStatus", c => c.Int());
            AddColumn("dbo.Subscriptions", "SubscriptionStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "SubscriptionStatus");
            DropColumn("dbo.AspNetUsers", "AccountStatus");
            DropColumn("dbo.AspNetUsers", "id_Subscription");
        }
    }
}
