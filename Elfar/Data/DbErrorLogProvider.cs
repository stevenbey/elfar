using System.Collections.Generic;
using System.Data;

namespace Elfar.Data
{
    public abstract class DbErrorLogProvider : IErrorLogProvider
    {
        static DbErrorLogProvider()
        {
            Settings = ErrorLogProvider.Settings as Settings ?? new Settings { Application = ErrorLogProvider.Settings.Application };
        }
        
        public virtual void Delete(int id)
        {
            using (var conn = Connection)
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = Scripts.Delete;
                SetIDParameter(cmd, id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public virtual void Save(ErrorLog errorLog)
        {
            using(var conn = Connection)
            using(var cmd = conn.CreateCommand())
            {
                cmd.CommandText = Scripts.Save;
                SetSaveParameters(cmd, errorLog);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected virtual void SetSaveParameters(IDbCommand command, ErrorLog errorLog)
        {
            SetIDParameter(command, errorLog.ID);

            var json = errorLog.Json.Compress();
            var parameter = command.CreateParameter();
            parameter.ParameterName = "Json";
            parameter.Size = json.Length;
            parameter.Value = json;
            parameter.DbType = DbType.String;
            command.Parameters.Add(parameter);
        }

        static void SetIDParameter(IDbCommand command, int id)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = "ID";
            parameter.DbType = DbType.Int32;
            parameter.Value = id;
            command.Parameters.Add(parameter);
        }

        public IEnumerable<ErrorLog> All
        {
            get
            {
                var errorLogs = new List<ErrorLog>();
                using (var conn = Connection)
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = Scripts.All;
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        errorLogs.Add(new ErrorLog
                        {
                            ID = reader.GetInt32(0),
                            Json = reader.GetString(1).Decompress()
                        });
                    }
                }
                return errorLogs;
            }
        }

        protected abstract IDbConnection Connection { get; }

        protected virtual SqlScripts Scripts
        {
            get { return scripts ?? (scripts = new SqlScripts()); }
            set { scripts = value; }
        }

        protected static readonly Settings Settings;
        
        SqlScripts scripts;

        protected class SqlScripts
        {
            public SqlScripts()
            {
                All = "SELECT * FROM " + Table;
                Delete = "DELETE FROM " + Table + " WHERE ID = @ID";
                Save = "INSERT INTO " + Table + " VALUES(@ID, @Json)";
            }

            public string All { get; set; }
            public string Delete { get; set; }
            public string Save { get; set; }

            protected static string Table
            {
                get { return Settings.Table; }
            }
        }
    }

    public abstract class DbErrorLogProvider<TConnection> : DbErrorLogProvider where TConnection : class, IDbConnection, new()
    {
        protected sealed override IDbConnection Connection
        {
            get
            {
                return new TConnection { ConnectionString = Settings.ConnectionString };
            }
        }
    }
}