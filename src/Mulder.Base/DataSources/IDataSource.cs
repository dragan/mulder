using System;
using System.Collections.Generic;
using System.IO;

using Mulder.Base.Domain;

namespace Mulder.Base.DataSources
{
	public interface IDataSource
	{
		void CreateLayout(string identifier, Stream content);
		void CreateItem(string identifier, Stream content);
		void CreateItem(string identifier, Stream content, string extension);
		void CreateItem(string identifier, Stream content, object meta);
		void CreateItem(string identifier, Stream content, string extension, object meta);
		IEnumerable<Item> GetItems();
		IEnumerable<Layout> GetLayouts();
	}
}
