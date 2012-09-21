using Microsoft.CSharp.RuntimeBinder;
using System;
using System.IO;

using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Mulder.Base.Filters
{
	public class RazorFilter : IFilter
	{
		public RazorFilter()
		{
		}

		public string Execute(string source, dynamic model)
		{
			string includesPath = GetIncludesPath(model);

			var serviceConfig = new TemplateServiceConfiguration {
				Resolver = new IncludesResolver(includesPath)
			};

			Razor.SetTemplateService(new TemplateService(serviceConfig));

			return Razor.Parse(source, model);
		}

		string GetIncludesPath(dynamic model)
		{
			string includesPath = string.Empty;

			try {
				var layoutMeta = model.Layout;
				
				if (layoutMeta.ContainsKey("includes_path"))
					includesPath = layoutMeta["includes_path"].ToString();
			}
			catch (RuntimeBinderException e) {
				// Just ignore the exception if it doesn't exist
			}
			
			return includesPath;
		}
	}

	class IncludesResolver : ITemplateResolver
	{
		string root;
		
		public IncludesResolver(string root)
		{
			this.root = root;
		}
		
		public string Resolve(string name)
		{
			string includesPath = Path.Combine(root, name);
			bool exists = false;

			foreach (var ext in new[] { ".cshtml", ".html", ".htm" }) {
				var testPath = String.Concat(includesPath, ext);
				exists = File.Exists(testPath);
				if (exists) {
					includesPath = testPath;
					break;
				}
			}

			return exists ? File.ReadAllText(includesPath) : String.Empty;
		}
	}
}
