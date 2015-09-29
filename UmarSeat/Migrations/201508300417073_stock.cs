namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        id_Stock = c.Int(nullable: false, identity: true),
                        stockName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id_Stock);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stocks");
        }
    }
}
