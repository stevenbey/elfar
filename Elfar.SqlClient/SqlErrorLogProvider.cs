using System.ComponentModel.Composition;
using System.Data.SqlClient;
using Elfar.Data;

namespace Elfar.SqlClient
{
    using Properties;

    [Export("Provider", typeof(IErrorLogProvider))]
    public sealed class SqlErrorLogProvider
        : DbErrorLogProvider<SqlConnection>
    {
        public SqlErrorLogProvider()
        {
            TryExecute(() =>
            {
                var builder = new SqlConnectionStringBuilder(ConnectionString);
                var catalog = builder.InitialCatalog;
                builder.InitialCatalog = "master";
                using(var conn = new SqlConnection(builder.ToString()))
                {
                    conn.Execute(string.Format(Resources.Database, catalog));
                }
            });
            TryExecute(() =>
            {
                using(var conn = Connection)
                {
                    conn.Execute(Resources.Table);
                    conn.Execute(Resources.Index);
                }
            });
        }
    }
}