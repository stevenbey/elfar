using System.ComponentModel.Composition;

namespace Elfar
{
    [InheritedExport]
    public interface IErrorLogProvider
    {
        void Delete(string id);
        void Save(ErrorLog.Data data);

        string Summaries { get; }
        string this[string id] { get; set; }
    }
}