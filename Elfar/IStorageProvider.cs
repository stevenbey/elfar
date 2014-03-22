using System.Collections.Generic;

namespace Elfar
{
    interface IStorageProvider
    {
        IEnumerable<ErrorLog.Storage> Items { get; }
    }
}