using System;
using System.IO;
using System.Text;

using Mulder.Base.Extensions;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Base.DataSources
{
	public class FileSystemUnified : IDataSource
	{
		const string LayoutsDirectoryName = "layouts";
		const string ContentDirectoryName = "content";
		
		readonly ILog log;
		readonly IFileSystem fileSystem;
		
		public FileSystemUnified(ILog log, IFileSystem fileSystem)
		{
			this.log = log;
			this.fileSystem = fileSystem;
		}
		
		public void CreateLayout(string identifier, Stream content)
		{
			CreateObject(LayoutsDirectoryName, identifier, content, ".html", null);
		}
		
		public void CreateItem(string identifier, Stream content)
		{
			CreateItem(identifier, content, ".html");
		}

		public void CreateItem(string identifier, Stream content, string extension)
		{
			CreateItem(identifier, content, extension, null);
		}
		
		public void CreateItem(string identifier, Stream content, object meta)
		{
			CreateItem(identifier, content, ".html", meta);
		}
		
		public void CreateItem(string identifier, Stream content, string extension, object meta)
		{
			CreateObject(ContentDirectoryName, identifier, content, extension, meta);
		}
		
		void CreateObject(string directoryName, string identifier, Stream content, string extension, object meta)
		{
			string fileName = CreateFileNameFromIdentifier(identifier, extension);
			string path = Path.Combine(directoryName, fileName);
			
			string parent = Path.GetDirectoryName(path);
			
			fileSystem.CreateDirectory(parent);
			
			CreateFile(path, content, meta);
		}
		
		string CreateFileNameFromIdentifier(string identifier, string extension)
		{
			return identifier == "/" ? "index.html" : identifier.Substring(1, identifier.Length - 2) + extension;
		}
		
		void CreateFile(string path, Stream content, object meta)
		{
			using (var memoryStream = new MemoryStream()) {
				if (meta != null) {
					WriteMetaHeader(meta, memoryStream);
				}
				
				content.CopyTo(memoryStream);
				
				memoryStream.Position = 0;
				
				fileSystem.WriteStreamToFile(path, memoryStream);
				
				log.InfoMessage("\tcreate {0}", path);
			}
		}
		
		void WriteMetaHeader(object meta, MemoryStream memoryStream)
		{
			string yamlHeader = meta.ToDictionary().ToYamlHeader();
			var utf8Encoding = new UTF8Encoding();
			byte[] buffer = utf8Encoding.GetBytes(yamlHeader + Environment.NewLine);
			memoryStream.Write(buffer, 0, buffer.Length);
		}
	}
}
