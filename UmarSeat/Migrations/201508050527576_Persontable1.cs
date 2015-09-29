namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Persontable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        id_Person = c.Int(nullable: false, identity: true),
                        userId = c.String(maxLength: 128),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        email = c.String(nullable: false),
                        mobileNumber = c.String(nullable: false),
                        id_subscription = c.Int(),
                        telephoneNumber = c.String(),
                        createdAt = c.DateTime(),
                        updatedAt = c.DateTime(),
                        accountStatus = c.String(),
                    })
                .PrimaryKey(t => t.id_Person)
                .ForeignKey("dbo.AspNetUsers", t => t.userId)
                .Index(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Persons", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.Persons", new[] { "userId" });
            DropTable("dbo.Persons");
        }
    }
}
