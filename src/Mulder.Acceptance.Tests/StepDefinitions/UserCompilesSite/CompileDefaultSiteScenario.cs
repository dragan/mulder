using System;
using System.Collections.Generic;
using System.IO;

using Shouldly;
using TechTalk.SpecFlow;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCompilesSite
{
	[Binding]
    public class CompileDefaultSiteScenario
	{
		string oldWorkingDirectory;
		string mulderPoweredSitePath;
		StringWriter writer;
		EntryPoint entryPoint;
		
		[BeforeScenario]
		public void BeforeScenario()
		{
			oldWorkingDirectory = Directory.GetCurrentDirectory();
			mulderPoweredSitePath = Path.Combine(Path.GetTempPath(), "mulder-default-compile-test");
			
			writer = new StringWriter();
			
			var log = new Log(writer, new LogLevel[] { LogLevel.Info, LogLevel.Error });
			
			var fileSystem = new FileSystem();
			
			var createCommands = new Dictionary<string, ICommand>();
			createCommands.Add("site", new CreateSiteCommand(log, fileSystem, new FileSystemUnified(log, fileSystem)));
			
			var commands = new Dictionary<string, ICommand>();
			commands.Add("create", new CreateCommand(log, createCommands));
			commands.Add("compile", new CompileCommand(log));
			
			entryPoint = new EntryPoint(log, commands);
		}
		
		[AfterScenario]
		public void CleanUp()
		{
			Directory.SetCurrentDirectory(oldWorkingDirectory);
			
			if (Directory.Exists(mulderPoweredSitePath))
				Directory.Delete(mulderPoweredSitePath, true);
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
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""create \[time] public/style\.css"" message")]
		public void Then_I_should_see_create_time_public_style_css_message()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""Site compiled in \[time]s\.""")]
		public void Then_I_should_see_site_compiled_in_time()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see configured output directory created")]
		public void Then_I_should_see_configured_output_directory_created()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the configured output directory should be named ""public""")]
		public void Then_the_configured_output_directory_should_be_named_public()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see created files in configured output directory")]
		public void Then_I_should_see_created_files_in_configured_output_directory()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the ""public/index\.html"" file should contain the default home content")]
		public void Then_the_public_index_html_file_should_contain_the_default_home_content()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the ""public/style\.css"" file should contain the default styles")]
		public void Then_the_public_style_css_file_should_contain_the_default_styles()
		{
			ScenarioContext.Current.Pending();
		}
	}
}
