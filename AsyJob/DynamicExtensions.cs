using Microsoft.CSharp.RuntimeBinder;

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
                if (typeof(T).IsValueType)
                {
                    var valueType = ToNonNullable(typeof(T));
                    return (T?)Convert.ChangeType(value, valueType);
                }
                return (T)value;
            }
            catch (InvalidCastException)
            {
                return default;
            }
        }

        private static Type ToNonNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}
