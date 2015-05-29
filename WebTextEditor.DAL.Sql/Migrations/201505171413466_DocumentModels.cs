using System.Data.Entity.Migrations;

namespace WebTextEditor.DAL.Sql.Migrations
{
    public partial class DocumentModels : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Document", new[] { "UserId" });
            DropPrimaryKey("dbo.Document");
            CreateTable(
                "dbo.DocumentCollaborator",
                c => new
                    {
                        DocumentId = c.String(nullable: false, maxLength: 32),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CaretPosition = c.Int(),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.DocumentId, t.UserId });
            
            CreateTable(
                "dbo.DocumentContent",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 32),
                        DocumentId = c.String(nullable: false, maxLength: 32),
                        Insert = c.Boolean(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.DocumentId);
            
            AlterColumn("dbo.Document", "Id", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Document", "Name", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Document", "Id");
            CreateIndex("dbo.Document", "Created");
            DropColumn("dbo.Document", "Text");
            DropColumn("dbo.Document", "Modified");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Document", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Document", "Text", c => c.String());
            DropIndex("dbo.Document", new[] { "Created" });
            DropIndex("dbo.DocumentContent", new[] { "DocumentId" });
            DropPrimaryKey("dbo.Document");
            AlterColumn("dbo.Document", "Name", c => c.String());
            AlterColumn("dbo.Document", "Id", c => c.String(nullable: false, maxLength: 128));
            DropTable("dbo.DocumentContent");
            DropTable("dbo.DocumentCollaborator");
            AddPrimaryKey("dbo.Document", "Id");
            CreateIndex("dbo.Document", "UserId");
        }
    }
}
