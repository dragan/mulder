using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

using DotLiquid;
using DotLiquid.FileSystems;

namespace Mulder.Base.Filters
{
	public class LiquidFilter : IFilter
	{
		public LiquidFilter()
		{
		}
		
		public string Execute(string source, dynamic model)
		{
			IDictionary<string, object> modelDictionary = model as IDictionary<string, object>;

			string includesPath = GetIncludesPath(modelDictionary);
			Hash data = Hash.FromDictionary(modelDictionary);
			Template template = Template.Parse(source);
			Template.FileSystem = new Includes(includesPath);
			string output = template.Render(data);
			
			return output;
		}
		
		string GetIncludesPath(IDictionary<string, object> modelDictionary)
		{
			string includesPath = string.Empty;
			
			if (modelDictionary.ContainsKey("layout")) {
				var layoutMeta = modelDictionary["layout"] as IDictionary<string, object>;
				
				if (layoutMeta.ContainsKey("includes_path"))
					includesPath = layoutMeta["includes_path"].ToString();
			}
			
			return includesPath;
		}
	}
	
	class Includes : IFileSystem
	{
		string root;
		
		public Includes(string root)
		{
			this.root = root;
		}
		
		public string ReadTemplateFile(Context context, string templateName)
		{
			string includePath = Path.Combine(root, templateName);
			
			if (File.Exists(includePath))
				return File.ReadAllText(includePath);
			
			return string.Empty;
		}
	}
}
