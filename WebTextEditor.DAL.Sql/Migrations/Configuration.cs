using System.Data.Entity.Migrations;

namespace WebTextEditor.DAL.Sql.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataContext context)
        {
        }
    }
}