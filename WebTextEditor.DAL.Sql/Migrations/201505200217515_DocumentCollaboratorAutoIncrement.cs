using System.Data.Entity.Migrations;

namespace WebTextEditor.DAL.Sql.Migrations
{
    public partial class DocumentCollaboratorAutoIncrement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentCollaborator", "Id", c => c.Long(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentCollaborator", "Id");
        }
    }
}
