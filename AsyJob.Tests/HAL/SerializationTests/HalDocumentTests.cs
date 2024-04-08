using AsyJob.Web.HAL.Json;
using AsyJob.Web.Tests.TestDoubles;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.Tests.HAL.SerializationTests
{
    internal class HalDocumentTests
    {

        [Test]
        public void Serialize_FullDocument_ShouldSerializeCorrectly()
        {
            //Arrange
            var person = new PersonHalDocument("Max Muster", 21);
            person.Links.Add(new LinkBuilder("/person/1")
                .SetName("self")
                .Build());
            person.Links.Add(new LinkBuilder("/person/1/address")
                .SetName("address")
                .Build());
            person.Embedded.Add(new Web.HAL.Embed(new AddressHalDocument("SimCity", "SimoleonStreet"), "address"));
            //Act
            var json = JsonConvert.SerializeObject(person, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = [ new JsonLinksConverter(), new JsonEmbeddedConverter() ],
                NullValueHandling = NullValueHandling.Ignore,
            });

            var json2 = JsonConvert.SerializeObject(person.Embedded, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = [ new JsonLinksConverter(), new JsonEmbeddedConverter() ],
                NullValueHandling = NullValueHandling.Ignore,
            });
            //Assert
            var expectedJson = """
                {
                    "name": "Max Muster",
                    "age": 21,
                    "_links": {
                        "self": { "href": "/person/1" },
                        "address": { "href": "/person/1/address" },
                    },
                    "_embedded": {
                        "address": {
                            "city": "SimCity",
                            "street": "SimoleonStreet" 
                        }
                    }
                }
                """;
            Assert.That(json, Is.EqualTo(expectedJson));
        }
    }
}
