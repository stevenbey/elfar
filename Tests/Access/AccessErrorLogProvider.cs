using System;
using Elfar.Data;
using NUnit.Framework;

namespace Elfar.Access.Tests
{

    [TestFixture]
    public class AccessErrorLogProvider
    {
        [Test]
        public void Save()
        {
            new Access.AccessErrorLogProvider().Save(new ErrorLog(null, new Exception("Test")));
        }
    }
}