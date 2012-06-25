using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Elfar.Data;

namespace Elfar.Access
{
    public sealed class AccessErrorLogProvider
        : FileBasedDbErrorLogProvider<OleDbConnection, AccessQueries>
    {
        public AccessErrorLogProvider(
            string connectionString = @default)
            : base(connectionString)
        {
            if (File.Exists(FilePath)) return;
            lock (key)
            {
                if (File.Exists(FilePath)) return;
                try
                {
                    var directory = Path.GetDirectoryName(FilePath);
                    if (!(directory == null || Directory.Exists(directory))) Directory.CreateDirectory(directory);
                    var builder = new OleDbConnectionStringBuilder(ConnectionString);
                    var dataSource = builder.DataSource;
                    if (dataSource.StartsWith(".") || dataSource.StartsWith(dataDirectoryMacroString))
                    {
                        builder.DataSource = FilePath;
                        ConnectionString = builder.ConnectionString;
                    }
                    var catalog = new ADOX.Catalog();
                    catalog.Create(ConnectionString);
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
                return conn.Query<AccessErrorLog>(Queries.Get.Replace("@ID", "'" + id + "'"))
                           .SingleOrDefault();
            }
        }
        public override IList<ErrorLog> List(int page = 0, int size = int.MaxValue)
        {
            page = Total - (page * size);
            size = page - size;
            using(var conn = Connection)
            {
                conn.Open();
                return conn.Query<AccessErrorLog>(string.Format(Queries.List, page, size))
                           .Select(e => (ErrorLog) e).ToList();
            }
        }
        public override void Save(ErrorLog errorLog)
        {
            using(var conn = Connection)
            {
                conn.Open();
                conn.Execute(Queries.Save, (AccessErrorLog) errorLog);
            }
            Total++;
        }

        const string @default = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=|DataDirectory|\Elfar.mdb";

        static readonly object key = new object();
    }
}