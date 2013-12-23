using System;
using System.Collections.Generic;
using System.Data;

namespace Elfar.Data
{
    public abstract class DbErrorLogProvider : IErrorLogProvider, IJsonProvider
    {
        static DbErrorLogProvider()
        {
            Settings = ErrorLogProvider.Settings as Settings ?? new Settings();
        }
        
        public void Delete(int id)
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
        public void Save(ErrorLog errorLog)
        {
            using(var conn = Connection)
            using(var cmd = conn.CreateCommand())
            {
                cmd.CommandText = Scripts.Save;
                SetSaveParameters(cmd, new ErrorLog.Storage(errorLog));
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected virtual void SetSaveParameters(IDbCommand command, ErrorLog.Storage data)
        {
            SetIDParameter(command, data.ID);

            var json = data.Json.Compress();
            var parameter = command.CreateParameter();
            parameter.ParameterName = "Value";
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

        protected abstract IDbConnection Connection { get; }

        IEnumerable<ErrorLog> IErrorLogProvider.All
        {
            get { throw new NotImplementedException(); }
        }
        IEnumerable<string> IJsonProvider.Json
        {
            get
            {
                var list = new List<string>();
                using (var conn = Connection)
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = Scripts.All;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        while (reader.Read()) list.Add(reader.GetString(0).Decompress());
                }
                return list;
            }
        }

        protected static readonly Settings Settings;
        
        static class Scripts
        {
            public static readonly string All = "SELECT Value FROM " + Table;
            public static readonly string Delete = "DELETE FROM " + Table + " WHERE ID = @ID";
            public static readonly string Save = "INSERT INTO " + Table + " VALUES(@ID, @Value)";

           static string Table
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