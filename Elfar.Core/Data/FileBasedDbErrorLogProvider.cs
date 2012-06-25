using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Hosting;

namespace Elfar.Data
{
    public abstract class FileBasedDbErrorLogProvider<TConnection, TQueries>
            : DbErrorLogProvider<TConnection, TQueries>
              where TConnection : class, IDbConnection, new()
              where TQueries : IDbQueries, new()
    {
        protected FileBasedDbErrorLogProvider(
            string connectionString)
            : base(null, connectionString) {}

        protected string DataDirectory
        {
            get
            {
                var currentDomain = AppDomain.CurrentDomain;
                var dataDirectory = currentDomain.GetData(dataDirectoryMacroString.Trim('|')) as string;
                if(string.IsNullOrEmpty(dataDirectory)) dataDirectory = currentDomain.BaseDirectory;
                return dataDirectory;
            }
        }
        protected string FilePath
        {
            get
            {
                if(filePath == null)
                {
                    var builder = new DbConnectionStringBuilder { ConnectionString = ConnectionString };
                    var dataSource = (string) builder["Data Source"];
                    if(dataSource.StartsWith("~/")) filePath = HostingEnvironment.MapPath(dataSource);
                    else if(dataSource.StartsWith("."))
                        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dataSource.Substring(2));
                    else if(dataSource.StartsWith(dataDirectoryMacroString))
                    {
                        var index = dataDirectoryMacroString.Length;
                        if(dataSource[index] == 92) index++;
                        filePath = Path.Combine(DataDirectory, dataSource.Substring(index));
                    }
                    else filePath = dataSource;
                }
                return filePath;
            }
        }
        
        protected const string dataDirectoryMacroString = "|DataDirectory|";

        string filePath;
    }
}