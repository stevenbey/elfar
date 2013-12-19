using System.Data;
using System.Data.SqlClient;

namespace Elfar.Data.SqlClient
{
    public sealed class SqlErrorLogProvider : DbErrorLogProvider<SqlConnection>
    {
        protected override void SetSaveParameters(IDbCommand command, ErrorLog.Storage storage)
        {
            base.SetSaveParameters(command, storage);
            
            var parameter = (SqlParameter) command.Parameters[1];
            parameter.SqlDbType = SqlDbType.NVarChar;
            parameter.Size = ((string)parameter.Value).Length;
        }
    }
}