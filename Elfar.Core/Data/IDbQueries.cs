namespace Elfar.Data
{
    public interface IDbQueries
    {
        string Delete { get; }
        string Get { get; }
        string List { get; }
        string Save { get; }
    }
}