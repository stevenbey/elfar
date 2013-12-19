using System.Collections.Generic;

namespace Elfar
{
    interface IInternalErrorLogProvider
    {
        IEnumerable<string> Json { get; }
    }
}