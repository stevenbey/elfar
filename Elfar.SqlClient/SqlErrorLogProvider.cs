using System;
using System.Data.SqlClient;
using Elfar.Data;

namespace Elfar.SqlClient
{
    using Properties;

    public sealed class SqlErrorLogProvider
        : DbErrorLogProvider<SqlConnection>
    {
        public SqlErrorLogProvider(
            string application = null,
            string connectionString = null)
            : base(application, connectionString)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(ConnectionString);
                builder["Initial Catalog"] = "master";
                using(var conn = new SqlConnection(builder.ToString()))
                {
                    conn.Execute(Resources.Database);
                }
                using(var conn = Connection)
                {
                    conn.Execute(Resources.Table);
                    conn.Execute(Resources.Index);
                }
            }
            catch(Exception) {}
        }
    }
}