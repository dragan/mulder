using System;
using System.Linq;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base.Compilation;
using Mulder.Base.Domain;
using Mulder.Base.IO;
using Mulder.Base.Loading;
using Mulder.Base.Logging;

namespace Mulder.Tests.Base.Loading
{
	public class LoaderTests
	{
		[TestFixture]
		public class when_loading_site_data
		{
			ILog log;
			IFileSystem fileSystem;
			Loader loader;
			
			[SetUp]
			public void SetUp()
			{
				string fakeConfigFile = @"property_one: one";

				string fakeRulesFile = @"Compile(""*"", (context) => {
	// Do Nothing
});
Route(""*"", (context) => {
	return string.Empty;
});
Layout(""*"", ""layout-filter"");";
				
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				fileSystem.FileExists("config.yaml").Returns(true);
				fileSystem.ReadStringFromFile("config.yaml").Returns(fakeConfigFile);
				fileSystem.FileExists("Rules").Returns(true);
				fileSystem.ReadStringFromFile("Rules").Returns(fakeRulesFile);
				
				loader = new Loader(log, fileSystem);
			}

			[Test]
			public void should_check_if_a_config_file_exists()
			{
				loader.LoadSiteData();
				
				fileSystem.Received().FileExists("config.yaml");
			}

			[Test]
			public void should_read_config_file()
			{
				loader.LoadSiteData();
				
				fileSystem.Received().ReadStringFromFile("config.yaml");
			}
			
			[Test]
			public void should_set_site_configuration()
			{
				Site site = loader.LoadSiteData();
				
				site.Configuration.ShouldNotBe(null);
				site.Configuration["property_one"].ToString().ShouldBe("one");
			}
			
			[Test]
			public void should_load_items()
			{
				Site site = loader.LoadSiteData();
				
				site.Items.ShouldNotBe(null);
			}
			
			[Test]
			public void should_load_layouts()
			{
				Site site = loader.LoadSiteData();
				
				site.Layouts.ShouldNotBe(null);
			}
			
			[Test]
			public void should_check_if_a_rules_file_exists()
			{
				loader.LoadSiteData();
				
				fileSystem.Received().FileExists("Rules");
			}
			
			[Test]
			public void should_read_rules_file()
			{
				loader.LoadSiteData();
				
				fileSystem.Received().ReadStringFromFile("Rules");
			}
			
			[Test]
			public void should_load_rules()
			{
				Site site = loader.LoadSiteData();
				
				site.CompileRules.Count().ShouldBeGreaterThan(0);
				site.RouteRules.Count().ShouldBeGreaterThan(0);
				site.LayoutRules.Count().ShouldBeGreaterThan(0);
			}
		}
	}
}
