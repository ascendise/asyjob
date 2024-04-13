using AsyJob.Web.HAL;

namespace AsyJob.Web.Tests.TestDoubles
{
    internal class AddressHalDocument(string city, string street) : HalDocument
    {
        public string City { get; set; } = city;
        public string Street { get; set; } = street;
    }
}
