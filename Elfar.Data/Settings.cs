using System.Configuration;

namespace Elfar.Data
{
    public class Settings : Elfar.Settings
    {
        public string ConnectionString
        {
            get { return connectionString ?? (ConnectionString = this["ConnectionString"]); }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) value = null;
                var settings = ConnectionStrings[value ?? "Elfar"];
                if (settings != null) value = settings.ConnectionString;
                if (string.IsNullOrWhiteSpace(value)) value = DefaultConnectionString;
                connectionString = value;
            }
        }
        public string Schema
        {
            get { return schema ?? (schema = this["Schema"]); }
            set { schema = value; }
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