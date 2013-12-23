using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;

namespace Elfar.Data.Odbc
{
    public sealed class OdbcErrorLogProvider : DbErrorLogProvider<OdbcConnection>
    {
        protected override void SetDeleteParameters(IDbCommand command, int id)
        {
            base.SetDeleteParameters(command, id);

            UpdateCommandText(command);
        }
        protected override void SetSaveParameters(IDbCommand command, ErrorLog.Storage storage)
        {
            base.SetSaveParameters(command, storage);

            var parameter = (OdbcParameter) command.Parameters[1];
            parameter.OdbcType = OdbcType.NText;
            parameter.Size = ((string) parameter.Value).Length;

            UpdateCommandText(command);
        }
        
        static void UpdateCommandText(IDbCommand command)
        {
            command.CommandText = Regex.Replace(command.CommandText, @"\@[a-zA-Z]+", m => "?");
        }
    }
}