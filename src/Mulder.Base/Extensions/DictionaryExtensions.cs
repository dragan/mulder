using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Mulder.Base.Extensions
{
	public static class DictionaryExtensions
	{
		public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
		{
			var expando = new ExpandoObject();
			var expandoDictionary = (IDictionary<string, object>)expando;

			foreach (var kvp in dictionary) {
				if (kvp.Value is IDictionary<string, object>) {
					var expandoValue = ((IDictionary<string, object>)kvp.Value).ToExpando();
					expandoDictionary.Add(kvp.Key, expandoValue);
				}
				else if (kvp.Value is ICollection) {
					var itemList = new List<object>();
					foreach (var item in (ICollection)kvp.Value) {
						if (item is IDictionary<string, object>) {
							var expandoItem = ((IDictionary<string, object>) item).ToExpando();
							itemList.Add(expandoItem);
						}
						else {
							itemList.Add(item);
						}
					}

					expandoDictionary.Add(kvp.Key, itemList);
				}
				else {
					expandoDictionary.Add(kvp);
				}
			}

			return expando;
		}
	}
}
