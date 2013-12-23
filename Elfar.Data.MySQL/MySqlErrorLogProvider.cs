using System.Data;
using MySql.Data.MySqlClient;

namespace Elfar.Data.MySQL
{
    public sealed class MySqlErrorLogProvider : DbErrorLogProvider<MySqlConnection>
    {
        protected override void SetSaveParameters(IDbCommand command, ErrorLog.Storage data)
        {
            base.SetSaveParameters(command, data);

            var parameter = (MySqlParameter) command.Parameters[1];
            parameter.MySqlDbType = MySqlDbType.LongText;
            parameter.Size = ((string) parameter.Value).Length;
        }
    }
}