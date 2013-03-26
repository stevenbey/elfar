using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Elfar.Data;

namespace Elfar.Access
{
    public sealed class AccessErrorLogProvider : FileBasedDbErrorLogProvider<OleDbConnection>
    {
        public AccessErrorLogProvider()
        {
            if (File.Exists(FilePath)) return;
            lock (key)
            {
                if (File.Exists(FilePath)) return;
                TryExecute(() =>
                {
                    var directory = Path.GetDirectoryName(FilePath);
                    if(!(directory == null || Directory.Exists(directory))) Directory.CreateDirectory(directory);
                    var builder = new OleDbConnectionStringBuilder(ConnectionString);
                    var dataSource = builder.DataSource;
                    if(dataSource.StartsWith(".") || dataSource.StartsWith(dataDirectoryMacroString))
                    {
                        builder.DataSource = FilePath;
                        ConnectionString = builder.ConnectionString;
                    }
                    new ADOX.Catalog().Create(ConnectionString);
                    using(var conn = Connection)
                    {
                        conn.Execute(Properties.Resources.Table);
                    }
                });
            }
        }

        public override void Delete(Guid id)
        {
            using(var conn = Connection)
            {
                conn.Execute(Queries.Delete.Replace("@ID", "'" + id + "'"));
            }
        }
        public override ErrorLog Get(Guid id)
        {
            using(var conn = Connection)
            {
                return conn.Query<AccessErrorLog>(Queries.Get.Replace("@ID", "'" + id + "'")).SingleOrDefault();
            }
        }
        public override IList<ErrorLog> List()
        {
            using(var conn = Connection)
            {
                return new List<ErrorLog>(conn.Query<AccessErrorLog>(Queries.List, new { Application }).Select(e => (ErrorLog) e));
            }
        }
        public override void Save(ErrorLog errorLog)
        {
            using(var conn = Connection)
            {
                conn.Execute(Queries.Save, (AccessErrorLog) errorLog);
            }
        }

        protected override string DefaultConnectionString
        {
            get { return @default; }
        }

        const string @default = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=|DataDirectory|\Elfar.mdb";
    }
}