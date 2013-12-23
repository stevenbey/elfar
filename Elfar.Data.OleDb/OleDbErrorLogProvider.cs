using System.Data;
using System.Data.OleDb;

namespace Elfar.Data.OleDb
{
    public sealed class OleDbErrorLogProvider : DbErrorLogProvider<OleDbConnection>
    {
        protected override void SetSaveParameters(IDbCommand command, ErrorLog.Storage storage)
        {
            base.SetSaveParameters(command, storage);

            var parameter = (OleDbParameter) command.Parameters[1];
            parameter.OleDbType = OleDbType.LongVarWChar;
            parameter.Size = ((string) parameter.Value).Length;
        }
    }
}