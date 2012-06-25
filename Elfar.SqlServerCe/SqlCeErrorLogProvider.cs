using System;
using System.Data.SqlServerCe;
using System.IO;
using Elfar.Data;

namespace Elfar.SqlServerCe
{
    using Properties;

    public class SqlCeErrorLogProvider
        : FileBasedDbErrorLogProvider<SqlCeConnection, SqlCeQueries>
    {
        public SqlCeErrorLogProvider(
            string connectionString = @default)
            : base(connectionString)
        {
            if (File.Exists(FilePath)) return;
            lock (key)
            {
                if (File.Exists(FilePath)) return;
                try
                {
                    using (var engine = new SqlCeEngine(ConnectionString)) engine.CreateDatabase();
                    using (var conn = Connection)
                    {
                        conn.Open();
                        conn.Execute(Resources.Table);
                        conn.Execute(Resources.Index);
                    }
                }
                catch(Exception) {}
            }
        }

        const string @default = @"|DataDirectory|\Elfar.sdf";

        static readonly object key = new object();
    }
}