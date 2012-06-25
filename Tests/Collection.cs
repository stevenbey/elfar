using System.Collections.Specialized;
using System.Web;
using NUnit.Framework;

namespace Elfar.Tests
{
    [TestFixture]
    public class Collection
    {
        [Test]
        public void Ctor()
        {
            // Arrange, Act & Assert
            Assert.DoesNotThrow(() => new Elfar.Collection());
        }
        [Test]
        public new void ToString()
        {
            // Arrange
            var collection = new Elfar.Collection
            {
                { "a", "1" },
                { "b", "2" }
            };

            // Act
            var s = collection.ToString();

            // Assert
            Assert.That(s, Is.EqualTo(@"{""a"":""1"",""b"":""2""}"));
        }
        [Test]
        public void FromString()
        {
            // Arrange
            const string s = @"{""a"":""1"",""b"":""2""}";
            
            // Act
            Elfar.Collection collection = s;

            // Arrange
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection["a"], Is.EqualTo("1"));
            Assert.That(collection["b"], Is.EqualTo("2"));
        }
        [Test]
        public void FromNVC()
        {
            // Arrange
            var nvc = new NameValueCollection { { "a", "1" }, { "b", "2" } };

            // Act
            var collection = new Elfar.Collection{ nvc };

            // Arrange
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection["a"], Is.EqualTo("1"));
            Assert.That(collection["b"], Is.EqualTo("2"));
        }
        [Test]
        public void FromCookies()
        {
            // Arrange
            var cookies = new HttpCookieCollection
            {
                    new HttpCookie("a", "1"),
                    new HttpCookie("b", "2")
            };

            // Act
            var collection = new Elfar.Collection{ cookies };

            // Arrange
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection["a"], Is.EqualTo("1"));
            Assert.That(collection["b"], Is.EqualTo("2"));
        }
    }
}