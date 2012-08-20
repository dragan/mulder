using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Mulder.Base.DataSources;
using Mulder.Base.Domain;
using Mulder.Base.Exceptions;
using Mulder.Base.Extensions;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Base.Loading
{
	public class Loader : ILoader
	{
		readonly ILog log;
		readonly IFileSystem fileSystem;
		
		public Loader(ILog log, IFileSystem fileSystem)
		{
			this.log = log;
			this.fileSystem = fileSystem;
		}
		
		public Site LoadSiteData()
		{
			var configuration = LoadConfiguration();

			var dataSources = new List<IDataSource>();
			foreach (var dataSourceMeta in configuration.DataSourceMetas) {
				if (dataSourceMeta.Type == "filesystem_unified")
					dataSources.Add(new FileSystemUnified(log, fileSystem, configuration));

				// TODO: Instantiate and add other data sources based on configuration
			}
			
			var items = LoadItems(dataSources);
			var layouts = LoadLayouts(dataSources);
			var rules = LoadRules();
			
			return new Site(configuration, items, layouts, rules.CompileRules, rules.RouteRules, rules.LayoutRules);
		}

		IConfiguration LoadConfiguration()
		{
			// TODO: Error handling when loading configuration fails
			if (fileSystem.FileExists("config.yaml")) {
				string yaml = fileSystem.ReadStringFromFile("config.yaml");
				var deserializedYaml = yaml.DeserializeYaml()[0] as IDictionary<string, object>;
				return new Configuration(deserializedYaml);
			}

			return new Configuration(new Dictionary<string, object>());
		}
		
		IList<Item> LoadItems(IEnumerable<IDataSource> dataSources)
		{
			var items = new List<Item>();
			
			foreach (var dataSource in dataSources) {
				items.AddRange(dataSource.GetItems());
			}
			
			return items;
		}
		
		IList<Layout> LoadLayouts(IEnumerable<IDataSource> dataSources)
		{
			var layouts = new List<Layout>();
			
			foreach (var dataSource in dataSources) {
				layouts.AddRange(dataSource.GetLayouts());
			}
			
			return layouts;
		}
		
		Rules LoadRules()
		{
			if (fileSystem.FileExists("Rules")) {
				string ruleProxyPartial = fileSystem.ReadStringFromFile("Rules");
				
				var ruleProxyInvoker = new RuleProxyInvoker(ruleProxyPartial);
				ruleProxyInvoker.InvokeLoadRules();
				
				var compileRules = ruleProxyInvoker.GetCompileRules();
				var routeRules = ruleProxyInvoker.GetRouteRules();
				var layoutRules = ruleProxyInvoker.GetLayoutRules();
				
				return new Rules {
					CompileRules = compileRules,
					RouteRules = routeRules,
					LayoutRules = layoutRules
				};
			}
			
			return new Rules {
				CompileRules = new List<CompileRule>(),
				RouteRules = new List<RouteRule>(),
				LayoutRules = new List<LayoutRule>()
			};
		}
		
		class Rules
		{
			public IEnumerable<CompileRule> CompileRules { get; set; }
			public IEnumerable<RouteRule> RouteRules { get; set; }
			public IEnumerable<LayoutRule> LayoutRules { get; set; }
		}
		
		class RuleProxyInvoker
		{
			readonly string ruleProxyPartial;
			readonly object ruleProxyInstance;
			readonly IDictionary<string, MethodInfo> ruleProxyMethods;
			readonly IDictionary<string, PropertyInfo> ruleProxyProperties;
			
			public RuleProxyInvoker(string ruleProxyPartial)
			{
				this.ruleProxyPartial = ruleProxyPartial;
				
				Type ruleProxyType = GetRuleProxyType();
				ruleProxyInstance = CreateRuleProxyInstance(ruleProxyType);
				
				ruleProxyMethods = new Dictionary<string, MethodInfo> {
					{ "LoadRules", ruleProxyType.GetMethod("LoadRules") }
				};
				
				ruleProxyProperties = new Dictionary<string, PropertyInfo> {
					{ "CompileRules", ruleProxyType.GetProperty("CompileRules") },
					{ "RouteRules", ruleProxyType.GetProperty("RouteRules") },
					{ "LayoutRules", ruleProxyType.GetProperty("LayoutRules") }
				};
			}
			
			public void InvokeLoadRules()
			{
				ruleProxyMethods["LoadRules"].Invoke(ruleProxyInstance, null);
			}
			
			public IEnumerable<CompileRule> GetCompileRules()
			{
				return ruleProxyProperties["CompileRules"].GetValue(ruleProxyInstance, null) as IEnumerable<CompileRule>;
			}
			
			public IEnumerable<RouteRule> GetRouteRules()
			{
				return ruleProxyProperties["RouteRules"].GetValue(ruleProxyInstance, null) as IEnumerable<RouteRule>;
			}
			
			public IEnumerable<LayoutRule> GetLayoutRules()
			{
				return ruleProxyProperties["LayoutRules"].GetValue(ruleProxyInstance, null) as IEnumerable<LayoutRule>;
			}
			
			Type GetRuleProxyType()
			{
				Assembly assembly = CompileRuleProxy();
				
				return assembly.GetType("Mulder.Base.Domain.RuleProxy");
			}
			
			Assembly CompileRuleProxy()
			{
				var assembly = Assembly.GetExecutingAssembly();
				
				var csharpCodeProvider = CSharpCodeProvider.CreateProvider("CSharp");
				
				var compilerParameters = new CompilerParameters {
					GenerateInMemory = true,
					GenerateExecutable = false,
					IncludeDebugInformation = false,
					ReferencedAssemblies = { "System.dll", assembly.Location }
				};
				
				string source = string.Format(GetRuleProxySource(assembly), ruleProxyPartial);
				
				var compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, source);
				
				if (compilerResults.Errors.Count > 0) {
					throw new ErrorCompilingRulesException(compilerResults.Errors);
				} else {
					return compilerResults.CompiledAssembly;
				}
			}
			
			string GetRuleProxySource(Assembly assembly)
			{
				using (Stream resourceStream = assembly.GetManifestResourceStream("RuleProxy.cs"))
				using (StreamReader streamReader = new StreamReader(resourceStream)) {
					return streamReader.ReadToEnd();
				}
			}
			
			object CreateRuleProxyInstance(Type type)
			{
				return Activator.CreateInstance(type);
			}
		}
	}
	
	public interface ILoader
	{
		Site LoadSiteData();
	}
}
