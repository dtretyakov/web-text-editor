using System.Data.Entity.Migrations;

namespace WebTextEditor.DAL.Sql.Migrations
{
    public partial class DocumentContentValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentContent", "Value", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentContent", "Value");
        }
    }
}
