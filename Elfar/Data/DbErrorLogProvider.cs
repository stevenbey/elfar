using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Elfar.Data
{
    public abstract class DbErrorLogProvider : ErrorLogProvider
    {
        protected DbErrorLogProvider()
        {
            var connectionString = ConnectionString;
            if(string.IsNullOrWhiteSpace(connectionString)) connectionString = null;
            var connectionStrings = ConfigurationManager.ConnectionStrings;
            var settings = connectionStrings[connectionString ?? Properties.Resources.Elfar];
            ConnectionString = settings == null ? connectionString : settings.ConnectionString;
            if(string.IsNullOrWhiteSpace(ConnectionString) && connectionStrings.Count != 0)
                ConnectionString = connectionStrings[0].ConnectionString;
        }

        public override void Delete(Guid id)
        {
            using(var conn = Connection)
            {
                conn.Execute(Queries.Delete, new { ID = id });
            }
        }
        public override ErrorLog Get(Guid id)
        {
            using(var conn = Connection)
            {
                return conn.Query<DbErrorLog>(Queries.Get, new { ID = id }).SingleOrDefault();
            }
        }
        public override IList<ErrorLog> List()
        {
            using(var conn = Connection)
            {
                return new List<ErrorLog>(conn.Query<DbErrorLog>(Queries.List, new { Application }).Select(l => (ErrorLog) l));
            }
        }
        public override void Save(ErrorLog errorLog)
        {
            using(var conn = Connection)
            {
                conn.Execute(Queries.Save, (DbErrorLog) errorLog);
            }
        }

        protected abstract IDbConnection Connection { get; }
        protected virtual IDbQueries Queries
        {
            get { return queries ?? (queries = new DbQueries()); }
        }

        DbQueries queries;
    }

    public abstract class DbErrorLogProvider<TConnection> : DbErrorLogProvider where TConnection: class, IDbConnection, new()
    {
        protected sealed override IDbConnection Connection
        {
            get
            {
                return new TConnection { ConnectionString = ConnectionString };
            }
        }
    }
}