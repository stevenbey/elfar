using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Elfar
{
    [InheritedExport]
    public interface IErrorLogProvider
    {
        void Delete(Guid id);
        void Save(ErrorLog.Storage errorLog);

        string Summaries { get; }
        string this[Guid id] { get; set; }
    }
}