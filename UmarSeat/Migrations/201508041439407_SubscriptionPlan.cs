namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionPlan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.subscriptionPlans",
                c => new
                    {
                        id_SubscriptionPlan = c.Int(nullable: false, identity: true),
                        nameSubscriptionPlan = c.String(nullable: false),
                        duration = c.Int(nullable: false),
                        subscriptionDurationType = c.Int(nullable: false),
                        subscriptionPrice = c.Single(nullable: false),
                        createdAt = c.DateTime(nullable: false),
                        updatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_SubscriptionPlan);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.subscriptionPlans");
        }
    }
}
