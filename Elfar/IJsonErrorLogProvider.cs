using System.Collections.Generic;

namespace Elfar
{
    interface IJsonErrorLogProvider
    {
        IEnumerable<string> Json { get; }
    }
}