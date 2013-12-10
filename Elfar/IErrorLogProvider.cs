using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Elfar
{
    [InheritedExport]
    public interface IErrorLogProvider
    {
        void Delete(int id);
        void Save(ErrorLog errorLog);

        IEnumerable<ErrorLog> All { get; }
        Settings Settings { get; set; }
    }
}