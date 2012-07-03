namespace Elfar.Data
{
    using Properties;

    public class DbQueries
            : IDbQueries
    {
        public virtual string Delete
        {
            get { return Resources.Delete; }
        }
        public virtual string Get
        {
            get { return Resources.Get; }
        }
        public virtual string List
        {
            get { return Resources.List; }
        }
        public virtual string Save
        {
            get { return Resources.Save; }
        }
    }
}