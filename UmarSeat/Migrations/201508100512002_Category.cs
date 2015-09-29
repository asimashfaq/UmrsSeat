namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Category : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        id_Category = c.Int(nullable: false, identity: true),
                        categoryName = c.String(nullable: false),
                        id_Subscription = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_Category);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Categories");
        }
    }
}
