using System;
using System.Collections.Generic;

namespace Elfar
{
    public interface IErrorLogProvider
    {
        ErrorLog Get(Guid id);
        IList<ErrorLog> List();
        void Save(ErrorLog errorLog);

        string Application { get; }
    }
}