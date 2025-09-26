using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Globalization;
using System.Dynamic;
using LabXand.Core;

namespace LabXand.Extensions
{

    public static class DictionaryExtensions
    {

        public static void AddRange<T, TU>(this IDictionary<T, TU> values, IEnumerable<KeyValuePair<T, TU>> other)
        {
            foreach (var kvp in other)
            {
                if (values.ContainsKey(kvp.Key))
                {
                    throw new ArgumentException("An item with the same key has already been added.");
                }
                values.Add(kvp);
            }
        }

        public static void Merge(this IDictionary<string, object> instance, string key, object value, bool replaceExisting = true)
        {
            if (replaceExisting || !instance.ContainsKey(key))
            {
                instance[key] = value;
            }
        }

        public static void Merge(this IDictionary<string, object> instance, object values, bool replaceExisting = true)
        {
            instance.Merge(new RouteValueDictionary(values), replaceExisting);
        }

        public static void Merge<T, TU>(this IDictionary<T, TU> instance, IDictionary<T, TU> from, bool replaceExisting = true)
        {
            foreach (KeyValuePair<T, TU> keyValuePair in from)
            {
                if (replaceExisting || !instance.ContainsKey(keyValuePair.Key))
                {
                    instance[keyValuePair.Key] = keyValuePair.Value;
                }
            }
        }

        public static void AppendInValue(this IDictionary<string, object> instance, string key, string separator, object value)
        {
            instance[key] = !instance.ContainsKey(key) ? value.ToString() : (instance[key] + separator + value);
        }

        public static void PrependInValue(this IDictionary<string, object> instance, string key, string separator, object value)
        {
            instance[key] = !instance.ContainsKey(key) ? value.ToString() : (value + separator + instance[key]);
        }

        public static string ToAttributeString(this IDictionary<string, object> instance)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in instance)
            {
                object[] args = { HttpUtility.HtmlAttributeEncode(pair.Key), HttpUtility.HtmlAttributeEncode(pair.Value.ToString()) };
                builder.Append(" {0}=\"{1}\"".FormatWith(args));
            }
            return builder.ToString();
        }

        public static T GetValue<TK, T>(this IDictionary<TK, object> instance, TK key)
        {
            try
            {
                object val;
                if (instance != null && instance.TryGetValue(key, out val) && val != null)
                    return (T)Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception exc)
            {
                exc.Dump();
            }
            return default(T);
        }
        public static T GetValue<TK, T>(this IDictionary<TK, T> instance, TK key)
        {
            try
            {
                T val;
                if (instance != null && instance.TryGetValue(key, out val) && val != null)
                    return (T)Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception exc)
            {
                exc.Dump();
            }
            return default;
        }

        public static ExpandoObject ToExpandoObject(this IDictionary<string, object> source, bool castIfPossible = false)
        {
            Guard.ArgumentNotNull(source, "source");

            if (castIfPossible && source is ExpandoObject)
            {
                return source as ExpandoObject;
            }

            var result = new ExpandoObject();
            result.AddRange(source);

            return result;
        }

    }

}
