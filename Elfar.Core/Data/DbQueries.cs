namespace Elfar.Data
{
    using Properties;

    public abstract class DbQueries
            : IDbQueries
    {
        public virtual string Count
        {
            get { return Resources.Count; }
        }
        public virtual string Get
        {
            get { return Resources.Get; }
        }
        public abstract string List
        {
            get;
        }
        public virtual string Save
        {
            get { return Resources.Save; }
        }
    }
}