using System;

namespace Elfar
{
    public sealed class TestException : ApplicationException
    {
        internal TestException() : base(message) {}

        const string message = "This is a test exception that can be safely ignored.";
    }
}