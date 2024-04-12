using AsyJob.Web.HAL.AspNetCore;
using AsyJob.Web.Tests.TestDoubles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.Tests.HAL
{
    internal class LinkBuilderTests
    {
        [Test]
        public void FromController_HasRoute_ShouldSetCorrectLinkForRoute()
        {
            //Arrange
            //Act
            var link = LinkBuilder.New()
                .FromController(typeof(PersonStubController), "Get")
                .SetName("getPerson")
                .Build();
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(link.Href, Is.EqualTo("/api/person/withid/{id}"));
                Assert.That(link.Templated, Is.True);
            });
        }
    }
}
