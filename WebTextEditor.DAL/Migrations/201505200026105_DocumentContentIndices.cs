namespace WebTextEditor.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentContentIndices : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DocumentContent", new[] { "DocumentId" });
            DropPrimaryKey("dbo.DocumentContent");
            AlterColumn("dbo.DocumentContent", "Id", c => c.String(nullable: false, maxLength: 900));
            AddPrimaryKey("dbo.DocumentContent", new[] { "DocumentId", "Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DocumentContent");
            AlterColumn("dbo.DocumentContent", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.DocumentContent", "Id");
            CreateIndex("dbo.DocumentContent", "DocumentId");
        }
    }
}
