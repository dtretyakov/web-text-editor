namespace WebTextEditor.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Documents : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DocumentCollaborator", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentCollaborator", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
