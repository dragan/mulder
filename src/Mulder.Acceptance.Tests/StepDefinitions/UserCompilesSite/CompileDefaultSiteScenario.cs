using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Autofac;
using Shouldly;
using TechTalk.SpecFlow;

using Mulder.Acceptance.Tests.Helpers;
using Mulder.Base;
using Mulder.Base.Logging;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCompilesSite
{
	[Binding]
    public class CompileDefaultSiteScenario
	{
		Regex whitespaceRegex;
		string oldWorkingDirectory;
		string mulderPoweredSitePath;
		string tempTestPath;
		StringWriter writer;
		EntryPoint entryPoint;
		
		[BeforeScenario]
		public void BeforeScenario()
		{
			whitespaceRegex = new Regex(@"(\t|\r\n|\r|\n)+");
			
			oldWorkingDirectory = Directory.GetCurrentDirectory();
			mulderPoweredSitePath = Path.Combine(Path.GetTempPath(), "mulder-default-compile-test");
			
			tempTestPath = Path.Combine(Path.GetTempPath(), "mulder-test-compile-result-files");
			CreateTestFiles();
			
			writer = new StringWriter();
			
			IContainer container = Ioc.CreateAcceptanceTestsContainer(writer, new LogLevel[] { LogLevel.Info, LogLevel.Error });
			
			entryPoint = container.Resolve<EntryPoint>();
		}
		
		[AfterScenario]
		public void CleanUp()
		{
			Directory.SetCurrentDirectory(oldWorkingDirectory);
			
			if (Directory.Exists(mulderPoweredSitePath))
				Directory.Delete(mulderPoweredSitePath, true);
			
			if (Directory.Exists(tempTestPath))
				Directory.Delete(tempTestPath, true);
		}
		
		void CreateTestFiles()
		{
			TestFileCreation.CreateFileFromResourceName("TEST_DEFAULT_HOME_RESULT", Path.Combine(tempTestPath, "index.html"));
			TestFileCreation.CreateFileFromResourceName("TEST_DEFAULT_STYLE_RESULT", Path.Combine(tempTestPath, "style.css"));
		}
		
		[Given(@"I have just created a new mulder powered site")]
		public void Given_I_have_just_created_a_new_mulder_powered_site()
		{
			entryPoint.Run(new string[] { "create", "site", mulderPoweredSitePath });
		}
		
		[Given(@"I have changed directories into the new mulder powered site")]
		public void Given_I_have_changed_directories_into_the_new_mulder_powered_site()
		{
			Directory.SetCurrentDirectory(mulderPoweredSitePath);
		}
		
		[When(@"I run the compile command")]
		public void When_I_run_the_compile_command()
		{
			entryPoint.Run(new string[] { "compile" });
		}
		
		[Then(@"I should see ""Loading site data\.\.\."" message")]
		public void Then_I_should_see_loading_site_data_message()
		{
			writer.ToString().ShouldContain("Loading site data...");
		}
		
		[Then(@"I should see ""Compiling site\.\.\."" message")]
		public void Then_I_should_see_compiling_site_message()
		{
			writer.ToString().ShouldContain("Compiling site...");
		}
		
		[Then(@"I should see ""create \[time] public/index\.html"" message")]
		public void Then_I_should_see_create_time_public_index_html_message()
		{
			writer.ToString().ShouldContain("create public/index.html");
		}
		
		[Then(@"I should see ""create \[time] public/style\.css"" message")]
		public void Then_I_should_see_create_time_public_style_css_message()
		{
			writer.ToString().ShouldContain("create public/style.css");
		}
		
		[Then(@"I should see ""Site compiled in \[time]s\.""")]
		public void Then_I_should_see_site_compiled_in_time()
		{
			writer.ToString().ShouldContain("Site compiled in ");
		}
		
		[Then(@"I should see configured output directory created")]
		public void Then_I_should_see_configured_output_directory_created()
		{
			Directory.Exists("public").ShouldBe(true);
		}
		
		// TODO: Not needed, but can't regenerate since on MD 3.0, need to update SpecFlow
		[Then(@"the configured output directory should be named ""public""")]
		public void Then_the_configured_output_directory_should_be_named_public()
		{
			Directory.Exists("public").ShouldBe(true);
		}
		
		[Then(@"I should see created files in configured output directory")]
		public void Then_I_should_see_created_files_in_configured_output_directory()
		{
			File.Exists(Path.Combine("public", "index.html")).ShouldBe(true);
			File.Exists(Path.Combine("public", "style.css")).ShouldBe(true);
		}
		
		[Then(@"the ""public/index\.html"" file should contain the default home content")]
		public void Then_the_public_index_html_file_should_contain_the_default_home_content()
		{
			string expectedDefaultHomePageResult = whitespaceRegex.Replace(File.ReadAllText(Path.Combine(tempTestPath, "index.html")), "");
			string actualDefaultHomePageResult = whitespaceRegex.Replace(File.ReadAllText(Path.Combine("public", "index.html")), "");
			
			actualDefaultHomePageResult.ShouldBe(expectedDefaultHomePageResult);
		}
		
		[Then(@"the ""public/style\.css"" file should contain the default styles")]
		public void Then_the_public_style_css_file_should_contain_the_default_styles()
		{
			string expectedDefaultStylesheetResult = whitespaceRegex.Replace(File.ReadAllText(Path.Combine(tempTestPath, "style.css")), "");
			string actualDefaultStylesheetResult = whitespaceRegex.Replace(File.ReadAllText(Path.Combine("public", "style.css")), "");
			
			actualDefaultStylesheetResult.ShouldBe(expectedDefaultStylesheetResult);
		}
	}
}
