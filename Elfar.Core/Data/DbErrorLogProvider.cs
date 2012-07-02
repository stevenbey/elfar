using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Elfar.Data
{
    public abstract class DbErrorLogProvider<TConnection>
        : IErrorLogProvider
          where TConnection: class, IDbConnection, new()
    {
        protected DbErrorLogProvider(
            string application = null,
            string connectionString = null)
        {
            Application = application;
            if (string.IsNullOrWhiteSpace(connectionString)) connectionString = null;
            var connectionStrings = ConfigurationManager.ConnectionStrings;
            var settings = connectionStrings[connectionString ?? Properties.Resources.Elfar];
            ConnectionString = settings == null ? connectionString : settings.ConnectionString;
            if(string.IsNullOrWhiteSpace(ConnectionString) && connectionStrings.Count != 0)
                ConnectionString = connectionStrings[0].ConnectionString;
        }

        public virtual ErrorLog Get(Guid id)
        {
            using(var conn = Connection)
            {
                return conn.Query<DbErrorLog>(Queries.Get, new { ID = id }).SingleOrDefault();
            }
        }
        public virtual IList<ErrorLog> List()
        {
            using(var conn = Connection)
            {
                return new List<ErrorLog>(conn.Query<DbErrorLog>(Queries.List, new { Application }).Select(l => (ErrorLog) l));
            }
        }
        public virtual void Save(ErrorLog errorLog)
        {
            using(var conn = Connection)
            {
                conn.Execute(Queries.Save, (DbErrorLog) errorLog);
            }
        }

        public string Application { get; private set; }

        protected TConnection Connection
        {
            get
            {
                return new TConnection { ConnectionString = ConnectionString };
            }
        }
        protected string ConnectionString { get; set; }
        protected virtual DbQueries Queries
        {
            get { return queries ?? (queries = new DbQueries()); }
        }

        DbQueries queries;
    }
}