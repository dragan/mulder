using System;
using System.Collections.Generic;
using System.Linq;

using Mulder.Base.Domain;
using Mulder.Base.Extensions;
using Mulder.Base.Filters;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Base.Compilation
{
	public class Compiler : ICompiler
	{
		readonly ILog log;
		readonly IFileSystem fileSystem;
		readonly IFilterFactory filterFactory;
		
		public Compiler(ILog log, IFileSystem fileSystem, IFilterFactory filterFactory)
		{
			this.log = log;
			this.fileSystem = fileSystem;
			this.filterFactory = filterFactory;
		}

		public void Compile(Site site)
		{
			string outputDirectory = site.Configuration["OutputDirectory"].ToString();
			fileSystem.CreateDirectory(outputDirectory);
			
			CreateStaticFiles(site);
			
			IEnumerable<StaticFile> staticFiles = site.Items.SelectMany(item => item.StaticFiles);
			
			ApplyRules(staticFiles, site, outputDirectory);
			
			CompileStaticFiles(staticFiles, site);
		}
		
		void CreateStaticFiles(Site site)
		{
			foreach (Item item in site.Items) {
				CompileRule compileRule = site.GetCompilationRuleFor(item);
				
				if (compileRule != null)
					item.AddStaticFile(new StaticFile(item));
			}
		}
		
		void ApplyRules(IEnumerable<StaticFile> staticFiles, Site site, string outputDirectory)
		{
			foreach (StaticFile staticFile in staticFiles) {
				ApplyRouteRule(staticFile, site, outputDirectory);
				ApplyCompileRule(staticFile, site);
			}
		}
		
		void ApplyRouteRule(StaticFile staticFile, Site site, string outputDirectory)
		{
			RouteRule routeRule = site.GetRoutingRuleFor(staticFile.Item);
			string route = routeRule.ApplyTo(staticFile, site);
			staticFile.Path = outputDirectory + route;
		}
		
		void ApplyCompileRule(StaticFile staticFile, Site site)
		{
			CompileRule compileRule = site.GetCompilationRuleFor(staticFile.Item);
			compileRule.ApplyTo(staticFile, site);
		}
		
		void CompileStaticFiles(IEnumerable<StaticFile> staticFiles, Site site)
		{
			foreach (StaticFile staticFile in staticFiles) {
				
				ExecuteFilters(staticFile);
				ExecuteLayoutFilter(staticFile, site);
				
				if (staticFile.Item.IsBinary) {
					fileSystem.Copy(staticFile.Item.Meta["filename"].ToString(), staticFile.Path);
				} else {
					fileSystem.WriteStringToFile(staticFile.Path, staticFile.GetLastSnapShot());
				}
				
				log.InfoMessage("\tcreate {0}", staticFile.Path);
			}
		}
		
		void ExecuteFilters(StaticFile staticFile)
		{
			foreach (var filterName in staticFile.FilterNameQueue) {
				IFilter filter = filterFactory.CreateFilter(filterName);
				
				string source = staticFile.GetLastSnapShot();
				var arguments = new Dictionary<string, object> {
					{"item", staticFile.Item.Meta }
				};
				
				string result = filter.Execute(source, arguments);
				
				staticFile.CreateSnapShot(result);
			}
		}
		
		void ExecuteLayoutFilter(StaticFile staticFile, Site site)
		{
			if (staticFile.Layout != null) {
				var layoutRule = site.GetLayoutRuleFor(staticFile.Layout);
			
				IFilter filter = filterFactory.CreateFilter(layoutRule.FilterName);
				var arguments = new Dictionary<string, object> {
					{ "layout", staticFile.Layout.Meta },
					{ "item", staticFile.Item.Meta },
					{ "content", staticFile.GetLastSnapShot() }
				};
			
				string result = filter.Execute(staticFile.Layout.Content, arguments);
			
				staticFile.CreateSnapShot(result);
			}
		}
	}
	
	public interface ICompiler
	{
		void Compile(Site site);
	}
}
