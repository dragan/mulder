using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Mulder.Base.Domain;
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
		readonly IDictionary<string, object> configuration;
		
		public FileSystemUnified(ILog log, IFileSystem fileSystem, IDictionary<string, object> configuration)
		{
			this.log = log;
			this.fileSystem = fileSystem;
			this.configuration = configuration;
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
		
		public IEnumerable<Item> GetItems()
		{
			var items = new List<Item>();
			
			var fileObjects = GetFileObjects(ContentDirectoryName);
			foreach (var fileObject in fileObjects) {
				items.Add(new Item(
					fileObject.Identifier,
					fileObject.IsBinary,
					fileObject.Content,
					fileObject.Meta,
					fileObject.ModificationTime
				));
			}
			
			return items;
		}
		
		public IEnumerable<Layout> GetLayouts()
		{
			var layouts = new List<Layout>();
			
			var fileObjects = GetFileObjects(LayoutsDirectoryName);
			foreach (var fileObject in fileObjects) {
				layouts.Add(new Layout(
					fileObject.Identifier,
					fileObject.Content,
					fileObject.Meta,
					fileObject.ModificationTime
				));
			}
			
			return layouts;
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
		
		List<FileObject> GetFileObjects(string path)
		{
			var fileObjects = new List<FileObject>();
			
			IDictionary<string, string[]> fileExtensionsByFileBaseName = GroupFileExtensionsByFileBaseName(path);
			
			foreach (var kvp in fileExtensionsByFileBaseName) {
				string fileBaseName = kvp.Key;
				string metaFileExtension = kvp.Value[0];
				string contentFileExtension = kvp.Value[1];
				
				string metaFileName = BuildFileName(fileBaseName, metaFileExtension);
				string contentFileName = BuildFileName(fileBaseName, contentFileExtension);
				
				bool isBinary = IsBinary(contentFileName);
				
				ParsedFileData parsedFileData = !isBinary
					? ParseFiles(contentFileName, metaFileName) : new ParsedFileData { Content = "", Meta = new Dictionary<string, object>() };
				
				IDictionary<string, object> meta = new Dictionary<string, object>(parsedFileData.Meta) {
					{ "filename", contentFileName },
					{ "meta_filename", metaFileName},
					{ "extension", contentFileExtension }
				};
				
				string contentFileNameWithoutPath = contentFileName.RemovePathFromFileName(path);
				string identifier = GetIdentifierForFileName(contentFileNameWithoutPath);
				
				DateTime modificationTime = GetModificationTime(contentFileName, metaFileName);
				
				fileObjects.Add(new FileObject {
					Identifier = identifier,
					IsBinary = isBinary,
					Content = parsedFileData.Content,
					Meta = meta,
					ModificationTime = modificationTime
				});
			}
			
			return fileObjects;
		}
		
		IDictionary<string, string[]> GroupFileExtensionsByFileBaseName(string path)
		{
			IEnumerable<string> files = fileSystem.GetAllFiles(path).Where(fileName => {
				string pattern = @"~|.orig|.rej|.bak";
				var regex = new Regex(pattern, RegexOptions.IgnoreCase);
				
				return !regex.IsMatch(fileName);
			});
			
			var filesByFileBaseName = 
				from file in files 
				group file by GetFileBaseName(file) into fileGroup 
				select new {
					FileBaseName = fileGroup.Key,
					Files = fileGroup
				};
			
			var fileExtensionsByFileBaseName = new Dictionary<string, string[]>(filesByFileBaseName.Count());
			foreach (var fileGroup in filesByFileBaseName) {
				string metaFile = fileGroup.Files.Where(fileName => Path.GetExtension(fileName) == ".yaml").FirstOrDefault();
				string metaFileExtension = metaFile != null ? ".yaml" : null;
				
				string contentFile = fileGroup.Files.Where(fileName => Path.GetExtension(fileName) != ".yaml").FirstOrDefault();
				string contentExtension = contentFile != null ? Path.GetExtension(contentFile) : null;
				
				fileExtensionsByFileBaseName.Add(fileGroup.FileBaseName, new[] { metaFileExtension, contentExtension });
			}
			
			return fileExtensionsByFileBaseName;
		}
		
		string GetFileBaseName(string path)
		{
			return path.Substring(0, path.Length - Path.GetExtension(path).Length);
		}
		
		string BuildFileName(string fileBaseName, string extension)
		{
			if (extension == null)
				return string.Empty;
			
			if (extension == string.Empty)
				return fileBaseName;
			
			return fileBaseName + extension;
		}
		
		bool IsBinary(string fileName)
		{
			string extensionWithoutDot = Path.GetExtension(fileName).Substring(1);
			string[] textExtensions = configuration["TextExtensions"] as string[];
			return !textExtensions.Contains(extensionWithoutDot);
		}
		
		ParsedFileData ParseFiles(string rawContentFileName, string metaFileName)
		{
			IDictionary<string, object> meta = null;
			string fileContent = string.Empty;
			
			// If we have a stand alone meta file
			if (!string.IsNullOrEmpty(metaFileName)) {
				fileContent = !string.IsNullOrEmpty(rawContentFileName) ? fileSystem.ReadStringFromFile(rawContentFileName) : string.Empty;
				meta = fileSystem.ReadStringFromFile(metaFileName).DeserializeYaml()[0] as IDictionary<string, object>;
			}
			else {
				string rawFileContent = !string.IsNullOrEmpty(rawContentFileName) ? fileSystem.ReadStringFromFile(rawContentFileName) : string.Empty;
				meta = rawFileContent.DeserializeYamlHeader();
				fileContent = rawFileContent.ExcludeYamlHeader();
			}
			
			return new ParsedFileData { Content = fileContent, Meta = meta };
		}
		
		string GetIdentifierForFileName(string fileName)
		{
			string identifier = string.Empty;
			if (fileName.Contains("/index.html")) {
				// Remove /index.html from file name
				identifier = fileName.Replace(Path.DirectorySeparatorChar + Path.GetFileName(fileName), "");
			}
			else {
				// Remove extension from file name
				identifier = fileName.Substring(0, fileName.Length - Path.GetExtension(fileName).Length);
			}
			
			return identifier != string.Empty ? string.Format("/{0}/", identifier.Trim()) : string.Empty;
		}
		
		DateTime GetModificationTime(string contentFileName, string metaFileName)
		{
			DateTime? metaModificationTime = null;
			DateTime? contentModificationTime = null;
			DateTime? modificationTime = null;
			
			if (!string.IsNullOrEmpty(metaFileName))
				metaModificationTime = fileSystem.GetLastWriteTimeUtc(metaFileName);
			
			if (!string.IsNullOrEmpty(contentFileName))
				contentModificationTime = fileSystem.GetLastWriteTimeUtc(contentFileName);
			
			if (metaModificationTime.HasValue && contentModificationTime.HasValue) {
				modificationTime = metaModificationTime > contentModificationTime ? metaModificationTime : contentModificationTime;
			}
			else if (metaModificationTime.HasValue) {
				modificationTime = metaModificationTime;
			}
			else if (contentModificationTime.HasValue) {
				modificationTime = contentModificationTime;
			}
			
			return modificationTime.Value;
		}
		
		class FileObject
		{
			public string Identifier { get; set; }
			public bool IsBinary { get; set; }
			public string Content { get; set; }
			public IDictionary<string, object> Meta { get; set; }
			public DateTime ModificationTime { get; set; }
		}
		
		class ParsedFileData
		{
			public string Content { get; set; }
			public IDictionary<string, object> Meta { get; set; }
		}
	}
}
