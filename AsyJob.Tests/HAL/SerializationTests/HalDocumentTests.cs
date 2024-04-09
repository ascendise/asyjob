using AsyJob.Web.HAL;
using AsyJob.Web.HAL.Json;
using AsyJob.Web.Tests.TestDoubles;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
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

        // FIXME: Currently, the converters write a null value for the property, but it doesn't get ignored, even
        // though NullValueHandling.Ignore is set... 
        // I am leaving it for now, as it doesn't hurt, but is still annoying
        [Test]
        public void Serialize_FullDocument_ShouldSerializeCorrectly()
        {
            //Arrange
            var person = new PersonHalDocument("Max Muster", 21);
            person.Links.Add("self", new LinkBuilder("/person/1")
                .Build());
            person.Links.Add("address", new LinkBuilder("/person/1/address")
                .Build());
            person.Embedded.Add(new Embed("address", new AddressHalDocument("SimCity", "SimoleonStreet")));
            //Act
            var json = JsonConvert.SerializeObject(person, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = [new JsonLinksConverter(), new JsonEmbeddedConverter()],
                NullValueHandling = NullValueHandling.Ignore,
            }); ;
            //Assert
            var expectedJson = """
                {
                        "name": "Max Muster",
                        "age": 21,
                        "_links": {
                            "self": { "href": "/person/1" },
                            "address": { "href": "/person/1/address" }
                        },
                        "_embedded": {
                            "address": {
                                "city": "SimCity",
                                "street": "SimoleonStreet",
                                "_links": null,
                                "_embedded": null
                            }
                        }
                }
                """;
            Assert.That(Minify(json), Is.EqualTo(Minify(expectedJson)));
        }

        private static string Minify(string s)
            => s.Replace(" ", "").Replace("\r", "").Replace("\n", "");
    }
}
