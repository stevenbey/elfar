using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Elfar
{
    [InheritedExport]
    public interface IErrorLogProvider : IEnumerable<ErrorLog>
    {
        void Delete(int id);
        void Save(ErrorLog errorLog);
    }
}