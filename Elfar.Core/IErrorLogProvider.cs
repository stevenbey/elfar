using System;
using System.Collections.Generic;

namespace Elfar
{
    public interface IErrorLogProvider
    {
        ErrorLog Get(Guid id);
        IList<ErrorLog> List(int page = 0, int size = int.MaxValue);
        void Save(ErrorLog errorLog);

        string Application { get; }
        int Total { get; }
    }
}