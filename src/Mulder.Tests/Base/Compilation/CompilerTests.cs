using System;
using System.Collections.Generic;
using System.Linq;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base.Compilation;
using Mulder.Base.Domain;
using Mulder.Base.Filters;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Tests.Base.Compilation
{
	public class CompilerTests
	{
		[TestFixture]
		public class when_compiling_a_site
		{
			Site site;
			ILog log;
			IFileSystem fileSystem;
			IFilterFactory filterFactory;
			IFilter filter;
			Compiler compiler;
			
			[SetUp]
			public void SetUp()
			{
				site = LoadFakeSite();
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				filterFactory = Substitute.For<IFilterFactory>();
				filter = Substitute.For<IFilter>();
				filterFactory.CreateFilter(Arg.Any<string>()).Returns(filter);
				
				compiler = new Compiler(log, fileSystem, filterFactory);
			}
			
			[Test]
			public void should_create_configured_output_directory()
			{
				compiler.Compile(site);
				
				fileSystem.Received().CreateDirectory(site.Configuration.OutputDirectory);
			}
			
			[Test]
			public void should_create_static_file_proxies_for_matched_items()
			{
				compiler.Compile(site);
				
				foreach (Item item in site.Items) {
					item.StaticFiles.Count().ShouldBe(1);
				}
			}
			
			[Test]
			public void should_set_path_on_each_static_file()
			{
				string expected = site.Configuration.OutputDirectory + "/file.html";
				
				compiler.Compile(site);
				
				var staticFiles = site
					.Items
					.SelectMany(item => item.StaticFiles)
					.Where(staticFile => staticFile.Path == expected);
				
				staticFiles.Count().ShouldBe(3);
			}
			
			[Test]
			public void should_set_filter_on_each_static_file()
			{
				compiler.Compile(site);
				
				var staticFiles = site.Items.SelectMany(item => item.StaticFiles);
				
				foreach (StaticFile staticFile in staticFiles) {
					string filterName = staticFile.FilterNameQueue.Dequeue();
					filterName.ShouldBe("filter");
				}
			}
			
			[Test]
			public void should_set_layout_on_each_static_file()
			{
				var expectedLayout = site.Layouts.Single();
				
				compiler.Compile(site);
				
				var staticFiles = site.Items.SelectMany(item => item.StaticFiles);
				
				foreach (StaticFile staticFile in staticFiles) {
					staticFile.Layout.ShouldBe(expectedLayout);
				}
			}
			
			[Test]
			public void should_create_filter_for_static_file_filters()
			{
				compiler.Compile(site);
				
				filterFactory.Received(3).CreateFilter("filter");
			}
			
			[Test]
			public void should_execute_filter_for_static_file_filters()
			{
				compiler.Compile(site);
				
				var staticFiles = site.Items.SelectMany(item => item.StaticFiles);
				foreach (StaticFile staticFile in staticFiles) {
					filter.Received().Execute("item-content", Arg.Any<IDictionary<string, object>>());
				}
			}
			
			[Test]
			public void should_create_filter_for_static_file_layout_filter()
			{
				compiler.Compile(site);
				
				filterFactory.Received(3).CreateFilter("layout-filter");
			}
			
			[Test]
			public void should_execute_filter_for_static_file_layout_filter()
			{
				compiler.Compile(site);
				
				var staticFiles = site.Items.SelectMany(item => item.StaticFiles);
				foreach (StaticFile staticFile in staticFiles) {
					filter.Received().Execute("layout-content", Arg.Any<IDictionary<string, object>>());
				}
			}
			
			[Test]
			public void should_have_three_snapshots_in_each_static_file()
			{
				compiler.Compile(site);
				
				var staticFiles = site.Items.SelectMany(item => item.StaticFiles);
				foreach (StaticFile staticFile in staticFiles) {
					staticFile.SnapShots.Count().ShouldBe(3);
				}
			}
			
			[Test]
			public void should_write_each_static_file_to_disk()
			{
				string expectedPath = site.Configuration.OutputDirectory + "/file.html";
				string expectedContent = "";
				
				compiler.Compile(site);
				
				fileSystem.Received(3).WriteStringToFile(expectedPath, expectedContent);
			}
			
			[Test]
			public void should_log_each_static_file_created()
			{
				string expectedPath = site.Configuration.OutputDirectory + "/file.html";
				
				compiler.Compile(site);
				
				log.Received(3).InfoMessage("\tcreate {0}", expectedPath);
			}
			
			static Site LoadFakeSite()
			{
				var configuration = Substitute.For<IConfiguration>();
				configuration.OutputDirectory.Returns("SomeOutputDirectory");
				
				var items = new List<Item> {
					new Item("/a/", false, "item-content", new Dictionary<string, object>(), DateTime.Now),
					new Item("/a/b/", false, "item-content", new Dictionary<string, object>(), DateTime.Now),
					new Item("/a/b/c/", false, "item-content", new Dictionary<string, object>(), DateTime.Now)
				};
				
				var layouts = new List<Layout> {
					new Layout("/layout/", "layout-content", new Dictionary<string, object>(), DateTime.Now)
				};
				
				var compileRules = new List<CompileRule> {
					new CompileRule("*", (context) => {
						context.WithFilter("filter").WithLayout("layout");
					})
				};
				
				var routeRules = new List<RouteRule> {
					new RouteRule("*", (context) => {
						return "/file.html";
					})
				};
				
				var layoutRules = new List<LayoutRule> {
					new LayoutRule("*", "layout-filter")
				};
				
				return new Site(configuration, items, layouts, compileRules, routeRules, layoutRules);
			}
		}
	}
}
