namespace BankManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "UserTypeId", "dbo.UserTypes");
            DropIndex("dbo.Users", new[] { "UserTypeId" });
            AddColumn("dbo.Users", "UserType", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "UpdatedOn", c => c.DateTime());
            DropColumn("dbo.Users", "UserTypeId");
            DropTable("dbo.UserTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        UserTypeId = c.Int(nullable: false, identity: true),
                        UserTypeName = c.String(),
                    })
                .PrimaryKey(t => t.UserTypeId);
            
            AddColumn("dbo.Users", "UserTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "UpdatedOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "UserType");
            CreateIndex("dbo.Users", "UserTypeId");
            AddForeignKey("dbo.Users", "UserTypeId", "dbo.UserTypes", "UserTypeId", cascadeDelete: true);
        }
    }
}
