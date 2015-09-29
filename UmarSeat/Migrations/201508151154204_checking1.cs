namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checking1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Persons", "id_subscription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Persons", "id_subscription", c => c.Int(nullable: false));
        }
    }
}
