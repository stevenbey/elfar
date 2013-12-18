using System.Data;
using System.Data.SqlClient;

namespace Elfar.Data.SqlClient
{
    using Properties;

    public sealed class SqlErrorLogProvider : DbErrorLogProvider<SqlConnection>
    {
        protected override SqlScripts CreaScripts()
        {
            return new TsqlScripts();
        }

        protected override void SetSaveParameters(IDbCommand command, ErrorLog errorLog)
        {
            base.SetSaveParameters(command, errorLog);
            
            var parameter = (SqlParameter) command.Parameters[1];
            parameter.SqlDbType = SqlDbType.NVarChar;
            parameter.Size = ((string)parameter.Value).Length;

            parameter = (SqlParameter) command.CreateParameter();
            parameter.ParameterName = "Application";
            parameter.SqlDbType = SqlDbType.NVarChar;
            parameter.Value = Settings.Application;
            command.Parameters.Insert(1, parameter);
        }

        sealed class TsqlScripts : SqlScripts
        {
            public TsqlScripts()
            {
                All += " WHERE Application IS NULL OR Application = @Application";
                Save = Save.Insert(Save.Length - 8, ", @Application");
            }
        }
    }
}