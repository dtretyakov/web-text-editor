using System.Data.Entity.Migrations;

namespace WebTextEditor.DAL.Sql.Migrations
{
    public partial class DocumentContent : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DocumentContent");
            AlterColumn("dbo.DocumentContent", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.DocumentContent", "Id");
            DropColumn("dbo.DocumentContent", "Insert");
            DropColumn("dbo.DocumentContent", "UserId");
            DropColumn("dbo.DocumentContent", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentContent", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.DocumentContent", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.DocumentContent", "Insert", c => c.Boolean(nullable: false));
            DropPrimaryKey("dbo.DocumentContent");
            AlterColumn("dbo.DocumentContent", "Id", c => c.String(nullable: false, maxLength: 32));
            AddPrimaryKey("dbo.DocumentContent", "Id");
        }
    }
}
