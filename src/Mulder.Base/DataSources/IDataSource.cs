using System;
using System.IO;

namespace Mulder.Base.DataSources
{
	public interface IDataSource
	{
		void CreateLayout(string identifier, Stream content);
		void CreateItem(string identifier, Stream content, string extension);
		void CreateItem(string identifier, Stream content, object meta);
	}
}
