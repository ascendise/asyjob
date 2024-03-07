using AsyJob;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests
{
    internal class DynamicExtensionTests
    {

        [Test]
        public void TryGetValue_HasValue_ShouldReturnValue()
        {
            //Arrange
            dynamic obj = new ExpandoObject();
            obj.MyValue = 14;
            //Act
            var value = DynamicExtensions.TryGetValue<int?>(obj, "MyValue");
            //Assert
            Assert.That(value, Is.EqualTo(14));
        }

        [Test]
        public void TryGetValue_ValueIsMissing_ShouldReturnNull()
        {
            //Arrange
            dynamic obj = new ExpandoObject();
            obj.MyValue = 14;
            //Act
            var value = DynamicExtensions.TryGetValue<int?>(obj, "MissingValue");
            //Assert
            Assert.That(value, Is.Null);
        }

        [Test]
        public void TryGetValue_HasValueButWrongType_ShouldReturnNull()
        {
            //Arrange
            dynamic obj = new ExpandoObject();
            obj.MyValue = 14;
            //Act
            var value = DynamicExtensions.TryGetValue<string?>(obj, "MyValue");
            //Assert
            Assert.That(value, Is.Null);
        }

        [Test]
        public void TryGetValue_LongToInt_ShouldReturnCastValue()
        {
            //Arrange
            dynamic obj = new ExpandoObject();
            obj.Long = (long)123;
            //Act
            var loong = DynamicExtensions.TryGetValue<int?>(obj, "Long");
            //Assert
            Assert.That(loong, Is.EqualTo(123));
        }
    }
}
