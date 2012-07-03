using System;
using System.Collections.Generic;

namespace Elfar
{
    public interface IErrorLogProvider
    {
        void Delete(Guid id);
        ErrorLog Get(Guid id);
        IList<ErrorLog> List();
        void Save(ErrorLog errorLog);

        string Application { get; }
    }
}