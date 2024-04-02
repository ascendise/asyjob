using AsyJob.Lib.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.Auth
{
    internal class RightTests
    {

        [Test]
        [TestCaseSource(nameof(ToStringCases))]
        public void ToString_SerializeRight_ShouldOutputCorrectString(Right input, string expected)
        {
            //Arrange
            //Act
            var output = input.ToString();
            //Assert
            Assert.That(output, Is.EqualTo(expected));
        }

        public static object[] ToStringCases =
        [
            new object[] { new Right("Resource", Operation.None), "Resource_0" },
            new object[] { new Right("Resource", Operation.Read), "Resource_r" },
            new object[] { new Right("Resource", Operation.Write), "Resource_w" },
            new object[] { new Right("Resource", Operation.Execute), "Resource_x" },
            new object[] { new Right("Resource", Operation.Read | Operation.Write), "Resource_rw" },
            new object[] { new Right("Resource", Operation.Execute | Operation.Write | Operation.Read), "Resource_rwx" },
        ];

        [Test]
        [TestCase("Resource_0", Operation.None)]
        [TestCase("Resource_r", Operation.Read)]
        [TestCase("Resource_w", Operation.Write)]
        [TestCase("Resource_x", Operation.Execute)]
        [TestCase("Resource_rw", Operation.Read | Operation.Write)]
        [TestCase("Resource_xwr", Operation.Execute | Operation.Write | Operation.Read)]
        public void Right_StringConstructor_ShouldCreateCorrectRight(string input, Operation expected)
        {
            //Arrange
            //Act
            var output = new Right(input);
            //Assert
            Assert.That(output.Resource, Is.EqualTo("Resource"));
            Assert.That(output.Ops, Is.EqualTo(expected));
        }

    }
}
