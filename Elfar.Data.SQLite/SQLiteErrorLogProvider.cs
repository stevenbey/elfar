using System.Data.SQLite;

namespace Elfar.Data.SQLite
{
    public sealed class SQLiteErrorLogProvider : DbErrorLogProvider<SQLiteConnection> {}
}