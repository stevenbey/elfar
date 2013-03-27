using System.Data.SqlClient;

namespace Elfar.Data.SqlClient
{
    using Properties;

    public sealed class SqlErrorLogProvider : DbErrorLogProvider<SqlConnection>
    {
        public SqlErrorLogProvider()
        {
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