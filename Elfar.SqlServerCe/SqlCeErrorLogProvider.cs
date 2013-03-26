using System.Data.SqlServerCe;
using System.IO;
using Elfar.Data;

namespace Elfar.SqlServerCe
{
    using Properties;

    public class SqlCeErrorLogProvider : FileBasedDbErrorLogProvider<SqlCeConnection>
    {
        public SqlCeErrorLogProvider()
        {
            if (File.Exists(FilePath)) return;
            lock (key)
            {
                if (File.Exists(FilePath)) return;
                TryExecute(() =>
                {
                    using (var engine = new SqlCeEngine(ConnectionString)) engine.CreateDatabase();
                    using (var conn = Connection)
                    {
                        conn.Execute(Resources.Table);
                        conn.Execute(Resources.Index);
                    }
                });
            }
        }

        protected override string DefaultConnectionString
        {
            get { return @default; }
        }

        const string @default = @"|DataDirectory|\Elfar.sdf";
    }
}