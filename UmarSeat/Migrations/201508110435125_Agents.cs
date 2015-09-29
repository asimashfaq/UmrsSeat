namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Agents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        id_Agent = c.Int(nullable: false, identity: true),
                        id_Subscription = c.Int(nullable: false),
                        id_Person = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_Agent)
                .ForeignKey("dbo.Persons", t => t.id_Person, cascadeDelete: true)
                .Index(t => t.id_Person);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Agents", "id_Person", "dbo.Persons");
            DropIndex("dbo.Agents", new[] { "id_Person" });
            DropTable("dbo.Agents");
        }
    }
}
