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
            Execute(() =>
            {
                var builder = new SqlConnectionStringBuilder(ConnectionString);
                var catalog = builder.InitialCatalog;
                builder.InitialCatalog = "master";
                using(var conn = new SqlConnection(builder.ToString()))
                {
                    conn.Execute(string.Format(Resources.Database, catalog));
                }
            });
            Execute(() =>
            {
                using(var conn = Connection)
                {
                    conn.Execute(Resources.Table);
                    conn.Execute(Resources.Index);
                }
            });
        }

        static void Execute(Action action)
        {
            try { action(); }
            catch(Exception) { }
        }

    }
}