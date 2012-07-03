using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Elfar.Data;

namespace Elfar.SQLite
{
    public class SQLiteErrorLogProvider
        : FileBasedDbErrorLogProvider<SQLiteConnection>
    {
        public SQLiteErrorLogProvider(
            string connectionString = @default)
            : base(connectionString)
        {
            if (File.Exists(FilePath)) return;
            lock (key)
            {
                if (File.Exists(FilePath)) return;
                try
                {
                    SQLiteConnection.CreateFile(FilePath);
                    using (var conn = Connection)
                    {
                        conn.Execute(Properties.Resources.Table);
                    }
                }
                catch(Exception) {}
            }
        }

        public override ErrorLog Get(Guid id)
        {
            using(var conn = Connection)
            {
                return conn.Query<SQLiteErrorLog>(Queries.Get, new { ID = id }).SingleOrDefault();
            }
        }
        public override IList<ErrorLog> List()
        {
            using(var conn = Connection)
            {
                return new List<ErrorLog>(conn.Query<SQLiteErrorLog>(Queries.List, new { Application }).Select(l => (ErrorLog) l));
            }
        }

        const string @default = @"|DataDirectory|\Elfar.db";

        static readonly object key = new object();
    }
}