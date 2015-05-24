using System;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;

namespace WebTextEditor.DAL.Infrastructure
{
    /// <summary>
    ///     Allows to retry on exceptions caused by request limit.
    /// </summary>
    public sealed class SqlAzureRequestLimitExecutionPolicy : SqlAzureExecutionStrategy
    {
        private const string limitReachedMessage = "The request limit for the database is 30 and has been reached.";

        protected override bool ShouldRetryOn(Exception exception)
        {
            var sqlException = exception.GetBaseException() as SqlException;
            if (sqlException != null && sqlException.Message.Contains(limitReachedMessage))
            {
                return true;
            }

            return base.ShouldRetryOn(exception);
        }
    }
}