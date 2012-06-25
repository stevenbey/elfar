using System;
using System.Data.SqlClient;
using Elfar.Data;

namespace Elfar.SqlClient
{
    using Properties;

    public sealed class SqlErrorLogProvider
        : DbErrorLogProvider<SqlConnection, SqlQueries>
    {
        public SqlErrorLogProvider(
            string application = null,
            string connectionString = null)
            : base(application, connectionString)
        {
            try
            {
                using (var conn = Connection)
                {
                    conn.Open();
                    conn.Execute(Resources.Table);
                    conn.Execute(Resources.Index);
                }
            }
            catch(Exception) {}
        }
    }
}