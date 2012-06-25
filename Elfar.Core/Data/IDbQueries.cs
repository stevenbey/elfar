namespace Elfar.Data
{
    public interface IDbQueries
    {
        string Count { get; }
        string Get { get; }
        string List { get; }
        string Save { get; }
    }
}