using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpHelper.Util {
    public static class DictionaryHelper {
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
                                                    TValue def = default(TValue)) =>
            dict.TryGetValue(key, out var value) ? value : def;

        public static TKey GetKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value)
            where TValue : IEquatable<TValue> =>
            dict.FirstOrDefault(p => p.Value.Equals(value)).Key;

        public static TValue Extract<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
                                                   TValue def = default(TValue)) {
            var value = dict.GetValue(key, def);
            if (dict.ContainsKey(key)) dict.Remove(key);
            return value;
        }

        public static object[,] To2DArray<TKey, TValue>(this Dictionary<TKey, TValue> dict) {
            var array = new object[dict.Count, 2];
            var list = dict.OrderBy(p=>p.Key).ToArray();
            for (var i = 0; i < list.Length; i++) {
                array[i, 0] = list[i].Key;
                array[i, 1] = list[i].Value;
            }
            return array;
        }

        public static Dictionary<string, object> ToDict(this object[,] array) {
            var dict = new Dictionary<string, object>();
            for (var i = 0; i < array.GetLength(0); i++)
                dict[array[i, 0].ToString()] = array[i, 1];
            return dict;
        }

        public static Dictionary<string, object> ToDict(this object[,] array, Func<object, object> func) {
            var dict = new Dictionary<string, object>();
            for (var i = 0; i < array.GetLength(0); i++)
                dict[array[i, 0].ToString()] = func(array[i, 1]);
            return dict;
        }

        public static Dictionary<string, object> ToUpper(this Dictionary<string, object> dict) {
            return dict.ToDictionary(p => p.Key.ToUpper(), p => p.Value);
        }
    }
}
