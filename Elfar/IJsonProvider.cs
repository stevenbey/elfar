using System.Collections.Generic;

namespace Elfar
{
    interface IJsonProvider
    {
        IEnumerable<string> Json { get; }
    }
}