using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Elfar.Data;

namespace Elfar.SQLite
{
    public class SQLiteErrorLogProvider
        : FileBasedDbErrorLogProvider<SQLiteConnection, SQLiteQueries>
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
                        conn.Open();
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
                conn.Open();
                return conn.Query<SQLiteErrorLog>(Queries.Get, new
                {
                    ID = id
                }).SingleOrDefault();
            }
        }
        public override IList<ErrorLog> List(int page = 0, int size = int.MaxValue)
        {
            using(var conn = Connection)
            {
                conn.Open();
                return conn.Query<SQLiteErrorLog>(Queries.List, new
                {
                    Application,
                    Page = page,
                    Size = size
                }).Select(e => (ErrorLog) e).ToList();
            }
        }

        public override int Total
        {
            get
            {
                if(total == null)
                {
                    using(var conn = Connection)
                    {
                        conn.Open();
                        total = (int) conn.Query<long>(Queries.Count, new { Application }).Single();
                    }
                }
                return (int) total;
            }
            protected set { total = value; }
        }

        const string @default = @"|DataDirectory|\Elfar.db";

        static readonly object key = new object();

        int? total;
    }
}