using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib
{
    public static class DictionaryExtensions
    {
        public static T? Get<T>(this IDictionary<string, object?> dict, string key) where T : struct
        {
            dict.TryGetValue(key, out object? result);
            if (result is null) return null;
            return Convert.ChangeType(result, typeof(T)) as T?;
        }
    }
}
