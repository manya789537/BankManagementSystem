namespace BankManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Address", c => c.String());
            AlterColumn("dbo.Users", "MobileNo", c => c.Long(nullable: false));
            DropColumn("dbo.Users", "BranchId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "BranchId", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "MobileNo", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "Address");
        }
    }
}
