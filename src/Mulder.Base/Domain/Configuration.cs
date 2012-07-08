using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Mulder.Base.DataSources;

namespace Mulder.Base.Domain
{
	public class Configuration : IConfiguration
	{
		readonly Dictionary<string, object> internalDictionary;
		readonly string outputDirectory = "public";
		readonly IEnumerable<string> textExtensions;
		readonly IEnumerable<string> indexFileNames;
		readonly List<DataSourceMeta> dataSourceMetas;

		public string OutputDirectory
		{
			get { return outputDirectory; }
		}

		public IEnumerable<string> TextExtensions
		{
			get { return textExtensions; }
		}

		public IEnumerable<string> IndexFileNames
		{
			get { return indexFileNames; }
		}

		public IEnumerable<DataSourceMeta> DataSourceMetas
		{
			get { return dataSourceMetas; }
		}

		public Configuration(IDictionary<string, object> dictionary)
		{
			internalDictionary = new Dictionary<string, object>(dictionary);

			// Set defaults
			textExtensions = new [] { "htm", "html", "css", "liquid", "js", "less", "markdown", "md", "text", "xhtml", "xml" };

			indexFileNames = new [] { "index.html" };

			dataSourceMetas = new List<DataSourceMeta> {
				new DataSourceMeta { Type = "filesystem_unified", ItemsRoot = "/", LayoutsRoot = "/" }
			};

			// Set based off config if values are supplied
			if (this.ContainsKey("output_directory"))
				outputDirectory = this["output_directory"].ToString();

			if (this.ContainsKey("text_extensions"))
				textExtensions = ((List<object>)this["text_extensions"]).Select(i => i.ToString()).ToList();

			if (this.ContainsKey("index_filenames"))
				indexFileNames = ((List<object>)this["index_filenames"]).Select(i => i.ToString()).ToList();

			if (this.ContainsKey("data_sources")) {
				dataSourceMetas.Clear();
				var dataSources = (List<object>)this["data_sources"];

				foreach (var dataSource in dataSources) {
					var meta = (IDictionary<string, object>)dataSource;
					string type = meta["type"].ToString();
					string itemsRoot = meta["items_root"].ToString();
					string layoutsRoot = meta["layouts_root"].ToString();

					dataSourceMetas.Add(new DataSourceMeta { Type = type, ItemsRoot = itemsRoot, LayoutsRoot = layoutsRoot });
				}
			}
		}

		object GetValue(string key)
		{
			if (internalDictionary.ContainsKey(key))
				return internalDictionary[key];

			throw new KeyNotFoundException();
		}

		// IDictionary<string, object> implementation
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return internalDictionary.GetEnumerator();
		}

		public void Remove(object key)
		{
			((IDictionary) internalDictionary).Remove(key);
		}

		public void Add(KeyValuePair<string, object> item)
		{
			((IDictionary<string, object>) internalDictionary).Add(item);
		}

		public void Add(object key, object value)
		{
			((IDictionary) internalDictionary).Add(key, value);
		}

		public bool Contains(object key)
		{
			return ((IDictionary) internalDictionary).Contains(key);
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return ((IDictionary<string, object>) internalDictionary).Contains(item);
		}

		public void Clear()
		{
			internalDictionary.Clear();
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((IDictionary<string, object>) internalDictionary).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return ((IDictionary<string, object>) internalDictionary).Remove(item);
		}

		object IDictionary.this[object key]
		{
			get
			{
				if (!(key is string))
					throw new NotSupportedException();

				return GetValue((string)key);
			}
			set
			{
				((IDictionary) internalDictionary)[key] = value;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return internalDictionary.GetEnumerator();
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IDictionary) internalDictionary).GetEnumerator();
		}

		// IDictionary implementation
		public int Count
		{
			get { return internalDictionary.Count; }
		}

		public object SyncRoot
		{
			get { return ((IDictionary) internalDictionary).SyncRoot; }
		}

		public bool IsSynchronized
		{
			get { return ((IDictionary) internalDictionary).IsSynchronized; }
		}

		public bool IsReadOnly
		{
			get { return ((IDictionary) internalDictionary).IsReadOnly; }
		}

		public bool IsFixedSize
		{
			get { return ((IDictionary) internalDictionary).IsFixedSize; }
		}

		public object this[string key]
		{
			get { return GetValue(key); }
			set { internalDictionary[key] = value; }
		}

		public ICollection<string> Keys
		{
			get { return internalDictionary.Keys; }
		}

		public ICollection<object> Values
		{
			get { return internalDictionary.Values; }
		}

		public void CopyTo(Array array, int index)
		{
			((IDictionary) internalDictionary).CopyTo(array, index);
		}

		public bool ContainsKey(string key)
		{
			return internalDictionary.ContainsKey(key);
		}

		public void Add(string key, object value)
		{
			internalDictionary.Add(key, value);
		}

		public bool Remove(string key)
		{
			return internalDictionary.Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return internalDictionary.TryGetValue(key, out value);
		}

		ICollection IDictionary.Values
		{
			get { return ((IDictionary) internalDictionary).Values; }
		}

		ICollection IDictionary.Keys
		{
			get { return ((IDictionary) internalDictionary).Keys; }
		}
	}

	public interface IConfiguration : IDictionary<string, object>, IDictionary
	{
		string OutputDirectory { get; }
		IEnumerable<string> TextExtensions { get; }
		IEnumerable<string> IndexFileNames { get; }
		IEnumerable<DataSourceMeta> DataSourceMetas { get; }
	}

	public class DataSourceMeta
	{
		public string Type { get; set; }
		public string ItemsRoot { get; set; }
		public string LayoutsRoot { get; set; }
	}
}
