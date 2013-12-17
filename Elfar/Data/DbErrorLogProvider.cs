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
            {
                conn.Execute("DELETE FROM [" + Table + "] WHERE [ID] = @ID", new { ID = id });
            }
        }
        public virtual void Save(ErrorLog errorLog)
        {
            using(var conn = Connection)
            {
                conn.Execute("INSERT INTO [" + Table + "] VALUES(@ID, @Json)", errorLog);
            }
        }

        public virtual IEnumerable<ErrorLog> All
        {
            get
            {
                using (var conn = Connection)
                {
                    return conn.Query<ErrorLog>("SELECT * FROM [" + Table + "]");
                }
            }
        }

        protected abstract IDbConnection Connection { get; }

        static string Table
        {
            get { return Settings.Table; }
        }

        protected static readonly Settings Settings;
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