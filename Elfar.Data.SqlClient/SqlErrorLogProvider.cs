using System.Data;
using System.Data.SqlClient;

namespace Elfar.Data.SqlClient
{
    public sealed class SqlErrorLogProvider : DbErrorLogProvider<SqlConnection>
    {
        protected override void SetSaveParameters(IDbCommand command, ErrorLog errorLog)
        {
            base.SetSaveParameters(command, errorLog);
            
            var parameter = (SqlParameter) command.Parameters[1];
            parameter.SqlDbType = SqlDbType.NVarChar;
            parameter.Size = ((string)parameter.Value).Length;
        }
    }
}