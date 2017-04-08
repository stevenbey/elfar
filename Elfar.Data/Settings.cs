using System.Configuration;

namespace Elfar.Data
{
    public class Settings : Elfar.Settings
    {
        private string connectionString, schema, table;

        public string ConnectionString
        {
            get => connectionString ?? (connectionString = Resolve(this[nameof(ConnectionString)]));
            set => connectionString = Resolve(value);
        }
        public string Schema
        {
            get => schema ?? (Schema = this[nameof(Schema)]);
            set => schema = string.IsNullOrWhiteSpace(value) ? "dbo" : value;
        }
        public string Table
        {
            get => table ?? (Table = this[nameof(Table)]);
            set => table = string.IsNullOrWhiteSpace(value) ? "Elfar_ErrorLogs" : value;
        }

        private static ConnectionStringSettingsCollection ConnectionStrings => ConfigurationManager.ConnectionStrings;

        private static string DefaultConnectionString => ConnectionStrings.Count == 0 ? null : ConnectionStrings[0].ConnectionString;

        private static string Resolve(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) value = null;
            var settings = ConnectionStrings[value ?? "Elfar"];
            if (settings != null) value = settings.ConnectionString;
            if (string.IsNullOrWhiteSpace(value)) value = DefaultConnectionString;
            return value;
        }
    }
}