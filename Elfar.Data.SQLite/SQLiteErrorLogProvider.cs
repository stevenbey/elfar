using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Elfar.Data.SQLite
{
    using Properties;

    public class SQLiteErrorLogProvider : FileBasedDbErrorLogProvider<SQLiteConnection>
    {
        public SQLiteErrorLogProvider()
        {
            if (File.Exists(FilePath)) return;
            lock (key)
            {
                if (File.Exists(FilePath)) return;
                TryExecute(() =>
                {
                    SQLiteConnection.CreateFile(FilePath);
                    using (var conn = Connection)
                    {
                        conn.Execute(Resources.Table);
                    }
                });
            }
        }

        public override Elfar.ErrorLog Get(Guid id)
        {
            using(var conn = Connection)
            {
                return conn.Query<ErrorLog>(Queries.Get, new { ID = id }).SingleOrDefault();
            }
        }
        public override IList<Elfar.ErrorLog> List()
        {
            using(var conn = Connection)
            {
                return new List<Elfar.ErrorLog>(conn.Query<ErrorLog>(Queries.List, new { Application }).Select(l => (Elfar.ErrorLog) l));
            }
        }

        protected override string DefaultConnectionString
        {
            get { return @default; }
        }

        const string @default = @"|DataDirectory|\Elfar.db";
    }
}