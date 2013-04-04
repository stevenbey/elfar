// ReSharper disable StaticFieldInGenericType
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Hosting;

namespace Elfar.Data
{
    public abstract class FileBasedDbErrorLogProvider<TConnection> : DbErrorLogProvider<TConnection> where TConnection : class, IDbConnection, new()
    {
        protected override void SetConnectionString(string value)
        {
            if(string.IsNullOrWhiteSpace(value)) value = DefaultConnectionString;
            if(value.StartsWith(dataDirectoryMacroString))
                value = "Data Source=" + ConnectionString;
            base.SetConnectionString(value);
        }

        protected string DataDirectory
        {
            get
            {
                var currentDomain = AppDomain.CurrentDomain;
                var dataDirectory = currentDomain.GetData(dataDirectoryMacroString.Trim('|')) as string;
                if(String.IsNullOrEmpty(dataDirectory)) dataDirectory = currentDomain.BaseDirectory;
                return dataDirectory;
            }
        }
        protected abstract string DefaultConnectionString { get; }
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
        protected static readonly object key = new object();
        
        string filePath;
    }
}