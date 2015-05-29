using System.Data.Entity.Migrations;

namespace WebTextEditor.DAL.Sql.Migrations
{
    public partial class DocumentCollaborator : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DocumentCollaborator");
            AddColumn("dbo.DocumentCollaborator", "ConnectionId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.DocumentCollaborator", "IsActive", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.DocumentCollaborator", new[] { "DocumentId", "ConnectionId" });
            DropColumn("dbo.DocumentCollaborator", "Updated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentCollaborator", "Updated", c => c.DateTime(nullable: false));
            DropPrimaryKey("dbo.DocumentCollaborator");
            DropColumn("dbo.DocumentCollaborator", "IsActive");
            DropColumn("dbo.DocumentCollaborator", "ConnectionId");
            AddPrimaryKey("dbo.DocumentCollaborator", new[] { "DocumentId", "UserId" });
        }
    }
}
