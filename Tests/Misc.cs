using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Web.Mvc;
using Beyond.Tests.Mvc;
using NUnit.Framework;

namespace Elfar.Tests
{

    [TestFixture]
    public class Misc
    {
        [Test]
        public void ValidateLuhn()
        {
            // Act & Assert
            Assert.That(new Luhn("356").IsValid, Is.True);
            Assert.That(new Luhn("3582").IsValid, Is.True);
            Assert.That(new Luhn("79927398713").IsValid, Is.True);
            Assert.That(new Luhn("49927398716").IsValid, Is.True);
            Assert.That(new Luhn("4222222222222").IsValid, Is.True);
            Assert.That(new Luhn("4129851284045568").IsValid, Is.True);
            Assert.That(new Luhn("4462789111211422").IsValid, Is.True);
            Assert.That(new Luhn("4462776126476165").IsValid, Is.True);
            Assert.That(new Luhn("4547426851489970").IsValid, Is.True);
        }

        [Test]
        public void GenerateLuhn()
        {
            // Act & Assert
            Assert.That(new Luhn().IsValid, Is.True);
            Assert.That(new Luhn(11).IsValid, Is.True);
        }

        [Test]
        public void FormAction()
        {
            // Arrange
            var selector = new FormActionAttribute();
            var context = Fakes.ControllerContext;
            context.HttpContext.Request.Form.Add("Action::Test", "");

            // Act &  Assert
            Assert.That(selector.IsValidName(context, null, new DynamicMethod("Test1", typeof(ActionResult), new Type[0])), Is.False);
            Assert.That(selector.IsValidName(context, null, new DynamicMethod("Test", typeof(ActionResult), new Type[0])), Is.True);
        }
    }

    public class FormActionAttribute
        : ActionNameSelectorAttribute
    {
        public override bool IsValidName(
            ControllerContext controllerContext,
            string name,
            MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request.Form["Action::" + methodInfo.Name] != null
                && !controllerContext.IsChildAction;
        }
    }

    public class Luhn
    {
        public Luhn(int length = 10)
        {
            Value = Generate(length);
        }
        public Luhn(string value)
        {
            Value = value;
        }

        public static string Generate(int length = 10)
        {
            var builder = new StringBuilder();
            var random = new Random();
            var sum = 0;
            for (var i = 1; i < length; i++)
            {
                var number = random.Next(10);
                sum += mapping[i & 1][number];
                builder.Insert(0, number);
            }
            builder.Append(sum * 9 % 10);
            return builder.ToString();
        }
        public static bool Validate(string value)
        {
            return value.Reverse()
                        .Select((c, i) => mapping[i & 1][int.Parse(c.ToString(CultureInfo.InvariantCulture))])
                        .Sum() % 10 == 0;
        }

        public bool IsValid { get { return Validate(Value); } }
        public string Value { get; private set; }

        static readonly int[][] mapping = new[]
        {
            new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            new[] {0, 2, 4, 6, 8, 1, 3, 5, 7, 9 }
        };
    }


}