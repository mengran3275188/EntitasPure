using System;
using System.Collections.Generic;
using System.Linq;

namespace DesperateDevs.Utils
{
	public static class DictionaryExtension
	{
		public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, params Dictionary<TKey, TValue>[] dictionaries)
		{
			return ((IEnumerable<Dictionary<TKey, TValue>>)dictionaries).Aggregate(dictionary, (Func<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>, Dictionary<TKey, TValue>>)((Dictionary<TKey, TValue> current, Dictionary<TKey, TValue> dict) => Enumerable.ToDictionary<KeyValuePair<TKey, TValue>, TKey, TValue>(Enumerable.Union<KeyValuePair<TKey, TValue>>((IEnumerable<KeyValuePair<TKey, TValue>>)current, (IEnumerable<KeyValuePair<TKey, TValue>>)dict), (Func<KeyValuePair<TKey, TValue>, TKey>)((KeyValuePair<TKey, TValue> kv) => kv.Key), (Func<KeyValuePair<TKey, TValue>, TValue>)((KeyValuePair<TKey, TValue> kv) => kv.Value))));
		}
	}
}
