namespace UmarSeat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        id_UserRroles = c.Int(nullable: false, identity: true),
                        userRolesType = c.String(),
                        userRolesName = c.String(),
                    })
                .PrimaryKey(t => t.id_UserRroles);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRoles");
        }
    }
}
