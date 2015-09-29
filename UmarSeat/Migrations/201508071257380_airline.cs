namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class airline : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.airLines",
                c => new
                    {
                        id_AirLine = c.Int(nullable: false, identity: true),
                        airlineName = c.String(nullable: false),
                        airlineContactPersonId = c.Int(nullable: false),
                        id_Subscription = c.Int(nullable: false),
                        createdAt = c.DateTime(nullable: false),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.id_AirLine)
                .ForeignKey("dbo.Subscriptions", t => t.id_Subscription, cascadeDelete: true)
                .Index(t => t.id_Subscription);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.airLines", "id_Subscription", "dbo.Subscriptions");
            DropIndex("dbo.airLines", new[] { "id_Subscription" });
            DropTable("dbo.airLines");
        }
    }
}
