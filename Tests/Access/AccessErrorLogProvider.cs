using System;
using NUnit.Framework;

namespace Elfar.Access.Tests
{

    [TestFixture]
    public class AccessErrorLogProvider
    {
        [Test]
        public void Save()
        {
            new Access.AccessErrorLogProvider(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source=.\Elfar.mdb")
                .Save(new ErrorLog(null, new Exception("Test")));
        }
    }
}