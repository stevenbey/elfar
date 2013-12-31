using System.Configuration;

namespace Elfar.Data
{
    public class Settings : Elfar.Settings
    {
        public string ConnectionString
        {
            get { return connectionString ?? (connectionString = DefaultConnectionString); }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) value = null;
                var settings = ConnectionStrings[value ?? "Elfar"];
                if (settings != null) value = settings.ConnectionString;
                if (string.IsNullOrWhiteSpace(value)) value = DefaultConnectionString;
                connectionString = value;
            }
        }
        public string Schema { get; set; }
        public string Table { get; set; }

        static ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return ConfigurationManager.ConnectionStrings; }
        }
        static string DefaultConnectionString
        {
            get { return ConnectionStrings.Count == 0 ? null : ConnectionStrings[0].ConnectionString; }
        }

        string connectionString;
    }
}