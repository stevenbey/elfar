using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Elfar.Data
{
    public abstract class DbErrorLogProvider<TConnection, TQueries>
        : IErrorLogProvider
          where TConnection: class, IDbConnection, new()
          where TQueries: IDbQueries, new()
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
                conn.Open();
                return conn.Query<DbErrorLog>(Queries.Get, new
                {
                    ID = id
                }).SingleOrDefault();
            }
        }
        public virtual IList<ErrorLog> List(int page = 0, int size = int.MaxValue)
        {
            using(var conn = Connection)
            {
                conn.Open();
                return conn.Query<ErrorLog>(Queries.List, new
                {
                    Application,
                    Page = page,
                    Size = size
                });
            }
        }
        public virtual void Save(ErrorLog errorLog)
        {
            using(var conn = Connection)
            {
                conn.Open();
                conn.Execute(Queries.Save, (DbErrorLog) errorLog);
            }
            total++;
        }

        public string Application { get; private set; }
        public virtual int Total
        {
            get
            {
                if(total == null)
                {
                    using(var conn = Connection)
                    {
                        conn.Open();
                        total = conn.Query<int>(Queries.Count, new { Application }).Single();
                    }
                }
                return (int) total;
            }
            protected set { total = value; }
        }

        protected TConnection Connection
        {
            get
            {
                return new TConnection { ConnectionString = ConnectionString };
            }
        }
        protected string ConnectionString { get; set; }
        protected static TQueries Queries
        {
            get { return new TQueries(); }
        }

        int? total;
    }
}