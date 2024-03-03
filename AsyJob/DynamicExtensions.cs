using Microsoft.CSharp.RuntimeBinder;
using System.Dynamic;

namespace AsyJob
{
    public static class DynamicExtensions
    {
        public static T? TryGetValue<T>(dynamic obj, string key)
        {
            try
            {
                var value = TryGetValue(obj, key);
                return TryCast<T>(value);    
            }
            catch (RuntimeBinderException)
            {
                return default;
            }
        }

       private static object? TryGetValue(object obj, string key)
       {
           var type = obj.GetType();
           return type.GetProperty(key)?.GetValue(obj);
       }

       private static object? TryGetValue(IDictionary<string, object> dict, string key)
       {
            dict.TryGetValue(key, out var value);
            return value;
       }

        private static T? TryCast<T>(object? value)
        {
            if (value == null) return default;
            try
            {
                return (T)value;
            }
            catch (InvalidCastException)
            {
                return default;
            }
        }
    }
}
