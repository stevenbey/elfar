namespace Elfar.Data.SqlClient
{
    public class Settings : Data.Settings
    {
        public string Schema
        {
            get { return schema; }
            set { schema = (value ?? "").Trim('[', ']'); }
        }
        public override string Table
        {
            get { return table ?? (table = string.IsNullOrWhiteSpace(Schema) ? base.Table : string.Concat(Schema, ".", base.Table)); }
            set { base.Table = value; }
        }

        string schema;
        string table;
    }
}