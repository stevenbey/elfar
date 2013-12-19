using System.Data;
using System.Data.OleDb;

namespace Elfar.Data.Access
{
    public sealed class AccessErrorLogProvider : DbErrorLogProvider<OleDbConnection>
    {
        protected override void SetSaveParameters(IDbCommand command, ErrorLog.Storage storage)
        {
            base.SetSaveParameters(command, storage);

            ((OleDbParameter) command.Parameters[1]).OleDbType = OleDbType.LongVarWChar;
        }
    }
}