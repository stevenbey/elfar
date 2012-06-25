using System;

namespace Elfar
{
    class ErrorLogException
        : Exception
    {
        public ErrorLogException(
            Exception inner)
            : base("Elfar experienced a problem.", inner) {}
    }
}