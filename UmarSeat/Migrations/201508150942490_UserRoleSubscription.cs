namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRoleSubscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserRoles", "id_Subscription", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserRoles", "id_Subscription");
        }
    }
}
