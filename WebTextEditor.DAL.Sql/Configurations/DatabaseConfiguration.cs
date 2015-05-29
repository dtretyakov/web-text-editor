using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace WebTextEditor.DAL.Sql.Configurations
{
    public class DatabaseConfiguration : DbConfiguration
    {
        public DatabaseConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}