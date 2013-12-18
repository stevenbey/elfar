namespace Elfar.Data.SqlClient
{
    public class Settings : Data.Settings
    {
        public override string Table
        {
            get { return table ?? (table = string.IsNullOrWhiteSpace(Schema) ? base.Table : string.Concat(Schema, ".", base.Table)); }
            set { base.Table = value; }
        }

        public string Schema { get; set; }

        string table;
    }
}