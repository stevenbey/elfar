using Elfar.Data;

namespace Elfar.SqlClient
{
    public class SqlQueries
            : DbQueries
    {
        public override string List
        {
            get { return Properties.Resources.List; }
        }
    }
}