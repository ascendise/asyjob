using Amazon.Runtime.Internal;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests
{
    internal static class HttpResponseMessageTestUtils
    {
        public static async Task<T?> ReadProperty<T>(this HttpResponseMessage message, string key) where T : class
        {
            var response = JObject.Parse(await message.Content.ReadAsStringAsync())
                ?? throw new ArgumentException("Response has no body");
            var value = response[key];
            return value?.ToObject<T?>();
        }
    }
}
