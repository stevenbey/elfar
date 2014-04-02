namespace Elfar.Mvc.ActionResults
{
    internal class DetailResult : ErrorLogResult
    {
        public DetailResult(int id)
        {
            this.id = id;
        }

        protected override string Content
        {
            get { return null; /*ErrorLogProvider.Details.Single(i => i.ID == id).Detail;*/ }
        }

        readonly int id;
    }
}