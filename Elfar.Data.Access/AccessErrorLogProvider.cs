using System.Data;
using System.Data.OleDb;

namespace Elfar.Data.Access
{
    public sealed class AccessErrorLogProvider : DbErrorLogProvider<OleDbConnection>
    {
        protected override void SetSaveParameters(IDbCommand command, ErrorLog errorLog)
        {
            base.SetSaveParameters(command, errorLog);

            ((OleDbParameter) command.Parameters[1]).OleDbType = OleDbType.LongVarWChar;
        }
    }
}