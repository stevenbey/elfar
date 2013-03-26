using System.ComponentModel.Composition;

namespace Elfar
{
    [InheritedExport]
    public interface IErrorLogPlugin
    {
        void Execute(ErrorLog errorLog);
    }
}