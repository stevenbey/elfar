using System.Data;
using System.Data.SqlServerCe;

namespace Elfar.Data.SqlServerCe
{
    public class SqlCeErrorLogProvider : DbErrorLogProvider<SqlCeConnection>
    {
        protected sealed override void SetSaveParameters(IDbCommand command, ErrorLog.Storage storage)
        {
            base.SetSaveParameters(command, storage);
            
            var parameter = (SqlCeParameter) command.Parameters[1];
            parameter.SqlDbType = SqlDbType.NText;
            parameter.Size = ((string) parameter.Value).Length;
        }
    }
}