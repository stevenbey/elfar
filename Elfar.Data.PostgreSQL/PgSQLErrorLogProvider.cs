using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Elfar.Data.PostgreSQL
{
    public sealed class PgSQLErrorLogProvider : DbErrorLogProvider<NpgsqlConnection>
    {
        protected override void SetSaveParameters(IDbCommand command, ErrorLog.Storage data)
        {
            base.SetSaveParameters(command, data);

            var parameter = (NpgsqlParameter) command.Parameters[1];
            parameter.NpgsqlDbType = NpgsqlDbType.Text;
            parameter.Size = ((string) parameter.Value).Length;
        }
    }
}