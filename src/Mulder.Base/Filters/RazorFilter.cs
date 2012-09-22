using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.IO;

using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

using Mulder.Base.Compilation;

namespace Mulder.Base.Filters
{
	public class RazorFilter : IFilter
	{
		public RazorFilter()
		{
		}

		public string Execute(string source, FilterContext filterContext)
		{
			string includesPath = GetIncludesPath(filterContext.Layout);

			var serviceConfig = new TemplateServiceConfiguration {
				Resolver = new IncludesResolver(includesPath)
			};

			Razor.SetTemplateService(new TemplateService(serviceConfig));

			dynamic model = filterContext;

			return Razor.Parse(source, model);
		}

		string GetIncludesPath(IDictionary<string, object> layout)
		{
			string includesPath = string.Empty;

			if (layout != null) {
				if (layout.ContainsKey("includes_path")) {
					includesPath = layout["includes_path"].ToString();
				}
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
