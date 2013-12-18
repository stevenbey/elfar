using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            {
                conn.Execute("DELETE FROM " + Table + " WHERE ID = @ID", new { ID = id });
            }
        }
        public virtual void Save(ErrorLog errorLog)
        {
            using(var conn = Connection)
            {
                conn.Execute("INSERT INTO " + Table + "(ID, Json) VALUES(@ID, @Json)", (DbErrorLog) errorLog);
            }
        }
        
        public virtual IEnumerable<ErrorLog> All
        {
            get
            {
                using (var conn = Connection)
                {
                    return conn.Query<DbErrorLog>("SELECT * FROM " + Table).Select(l => (ErrorLog) l);
                }
            }
        }

        protected abstract IDbConnection Connection { get; }

        static string Table
        {
            get { return Settings.Table; }
        }

        protected static readonly Settings Settings;

        class DbErrorLog : DynamicParameters
        {
            public static explicit operator DbErrorLog(ErrorLog errorLog)
            {
                return new DbErrorLog
                {
                    ID = errorLog.ID,
                    Json = errorLog.Json.Compress()
                };
            }
            public static explicit operator ErrorLog(DbErrorLog errorLog)
            {
                return new ErrorLog
                {
                    ID = errorLog.ID,
                    Json = errorLog.Json.Decompress()
                };
            }

            public int ID
            {
                get { return id; }
                set { Add("ID", id = value); }
            }
            public string Json
            {
                get { return json; }
                set { Add("Json", json = value, DbType.String, size: value.Length + 1); }
            }

            int id;
            string json;
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