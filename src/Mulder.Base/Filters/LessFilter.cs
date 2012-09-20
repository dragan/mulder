using System;
using System.Collections.Generic;
using System.IO;

using dotless.Core;
using dotless.Core.Importers;
using dotless.Core.Input;
using dotless.Core.Parser;
using dotless.Core.Stylizers;

using Mulder.Base.IO;

namespace Mulder.Base.Filters
{
	public class LessFilter : IFilter
	{
		readonly IFileSystem fileSystem;
		
		public LessFilter(IFileSystem fileSystem)
		{
			this.fileSystem = fileSystem;
		}
		
		public string Execute(string source, dynamic model)
		{
			var item = model.Item as IDictionary<string, object>;
			string path = Path.GetDirectoryName(item["filename"].ToString());
			
			string result = Transform(source, path);
			
			return result;
		}
		
		string Transform(string source, string path)
		{
			var engine = CreateLessEngine(path);
			
			return engine.TransformToCss(source, "");
		}
		
		ILessEngine CreateLessEngine(string path)
		{
			var importer = new Importer(new MulderFileReader(fileSystem, path));
			var parser = new Parser(new PlainStylizer(), importer);
			var engine = new LessEngine(parser) { Compress = true };
			return engine;
		}
		
		class MulderFileReader : IFileReader
		{
			readonly IFileSystem fileSystem;
			readonly string path;
			
			public MulderFileReader(IFileSystem fileSystem, string path)
			{
				this.fileSystem = fileSystem;
				this.path = path;
			}
			
			public string GetFileContents(string filename)
			{
				return fileSystem.ReadStringFromFile(Path.Combine(path, filename));
			}
			
			public bool DoesFileExist(string filename)
			{
				return fileSystem.FileExists(Path.Combine(path, filename));
			}
		}
	}
}
