using System;
using System.Collections.Generic;
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
		
		public string Execute(string source, IDictionary<string, object> arguments)
		{
			string includesPath = GetIncludesPath(arguments);
			Hash data = Hash.FromDictionary(arguments);
			Template template = Template.Parse(source);
			Template.FileSystem = new Includes(includesPath);
			string output = template.Render(data);
			
			return output;
		}
		
		string GetIncludesPath(IDictionary<string, object> arguments)
		{
			string includesPath = string.Empty;
			
			if (arguments.ContainsKey("layout")) {
				var layoutMeta = arguments["layout"] as IDictionary<string, object>;
				
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
