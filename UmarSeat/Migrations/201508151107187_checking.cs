namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checking : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Persons", "id_subscription", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Persons", "id_subscription", c => c.Int());
        }
    }
}
