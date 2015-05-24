using System.Data.Entity;
using WebTextEditor.DAL.Infrastructure;

namespace WebTextEditor.DAL.Configurations
{
    public class DatabaseConfiguration : DbConfiguration
    {
        public DatabaseConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureRequestLimitExecutionPolicy());
        }
    }
}