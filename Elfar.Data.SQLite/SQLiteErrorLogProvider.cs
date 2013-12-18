using System.Data.SQLite;

namespace Elfar.Data.SQLite
{
    public class SQLiteErrorLogProvider : DbErrorLogProvider<SQLiteConnection> {}
}