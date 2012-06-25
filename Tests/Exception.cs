namespace Elfar.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class Exception
    {
        [Test]
        public void InnerException()
        {
            try
            {
                throw new System.Exception("first");
            }
            catch(System.Exception first)
            {
                try
                {
                    throw new System.Exception("inner", first);
                }
                catch (System.Exception inner)
                {
                    try
                    {
                        throw new System.Exception("outer", inner);
                    }
                    catch (System.Exception outer)
                    {
                        var s = outer.ToString();
                        Assert.That(s, Is.Not.Null);
                    }
                }
            }
        }
    }
}