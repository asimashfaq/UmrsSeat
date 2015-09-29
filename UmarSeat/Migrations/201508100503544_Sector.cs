namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sector : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sectors",
                c => new
                    {
                        id_Sector = c.Int(nullable: false, identity: true),
                        sectorName = c.String(nullable: false),
                        id_Subscription = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_Sector);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sectors");
        }
    }
}
