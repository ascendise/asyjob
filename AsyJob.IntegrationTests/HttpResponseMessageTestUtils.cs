using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests
{
    internal static class HttpResponseMessageTestUtils
    {
        public static async Task<T?> ReadProperty<T>(this HttpResponseMessage message, Func<dynamic, T> func)
        {
            dynamic response = JsonConvert.DeserializeObject(await message.Content.ReadAsStringAsync()) 
                ?? throw new ArgumentException("Response has no body");
            return func(response);
        }
    }
}
