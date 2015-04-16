using System;
using System.Configuration;

namespace Elfar.Data
{
    public class Settings : Elfar.Settings
    {
        static string Resolve(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) value = null;
            var settings = ConnectionStrings[value ?? "Elfar"];
            if (settings != null) value = settings.ConnectionString;
            if (string.IsNullOrWhiteSpace(value)) value = DefaultConnectionString;
            return value;
        }

        public string ConnectionString
        {
            get { return Resolve(connectionString ?? (ConnectionString = this["ConnectionString"])); }
            set { connectionString = value; }
        }
        public string Schema
        {
            get { return schema ?? (Schema = this["Schema"]); }
            set { schema = string.IsNullOrWhiteSpace(value) ? "dbo" : value; }
        }
        public string Table
        {
            get { return table ?? (Table = this["Table"]); }
            set { table = string.IsNullOrWhiteSpace(value) ? "Elfar_ErrorLogs" : value; }
        }
        
        static ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return ConfigurationManager.ConnectionStrings; }
        }
        static string DefaultConnectionString
        {
            get { return ConnectionStrings.Count == 0 ? null : ConnectionStrings[0].ConnectionString; }
        }

        string connectionString, schema, table;
    }
}