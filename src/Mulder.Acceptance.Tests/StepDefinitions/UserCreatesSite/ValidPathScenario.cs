using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using Shouldly;
using TechTalk.SpecFlow;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCreatesSite
{
	[Binding]
    public class ValidPathScenario
	{
		Regex regex;
		
		string tempTestPath;
		string validPath;
		StringWriter writer;
		EntryPoint entryPoint;
		ExitCode exitCode;
		
		[BeforeScenario]
		public void BeforeScenario()
		{
			regex = new Regex(@"(\t|\r\n|\r|\n)+");
			
			tempTestPath = Path.Combine(Path.GetTempPath(), "mulder-acceptance-test-files");
			CreateTestFiles();
			
			writer = new StringWriter();
			
			var log = new Log(writer, new LogLevel[] { LogLevel.Info, LogLevel.Error });
			
			var createCommands = new Dictionary<string, ICommand>();
			var fileSystem = new FileSystem();
			createCommands.Add("site", new CreateSiteCommand(log, fileSystem, new FileSystemUnified(log, fileSystem)));
			
			var commands = new Dictionary<string, ICommand>();
			commands.Add("create", new CreateCommand(log, createCommands));
			
			entryPoint = new EntryPoint(log, commands);
		}
		
		[AfterScenario]
		public void CleanUp()
		{
			if (Directory.Exists(validPath))
				Directory.Delete(validPath, true);
			
			if (Directory.Exists(tempTestPath))
				Directory.Delete(tempTestPath, true);
		}
		
		void CreateTestFiles()
		{
			CreateFileFromResourceName("TEST_DEFAULT_CONFIG", Path.Combine(tempTestPath, "config.yaml"));
			CreateFileFromResourceName("TEST_DEFAULT_RULES", Path.Combine(tempTestPath, "Rules"));
			CreateFileFromResourceName("TEST_DEFAULT_LAYOUT", Path.Combine(tempTestPath, "default.html"));
			CreateFileFromResourceName("TEST_DEFAULT_HOME_PAGE", Path.Combine(tempTestPath, "index.html"));
			CreateFileFromResourceName("TEST_DEFAULT_STYLE_SHEET", Path.Combine(tempTestPath, "stylesheet.css"));
		}
		
		[Given(@"I have a valid path")]
		public void Given_I_have_a_valid_path()
		{
			validPath = Path.Combine(Path.GetTempPath(), "default-mulder-site");
		}

		[When(@"I run the create site command with a valid path")]
		public void When_I_run_the_create_site_command_with_valid_path()
		{
			exitCode = entryPoint.Run(new string[] { "create", "site", validPath });
		}
		
		[Then(@"I should see ""create config\.yaml"" message")]
		public void Then_I_should_see_create_config_yaml_message()
		{
			writer.ToString().ShouldContain("create config.yaml");
		}
		
		[Then(@"I should see ""create Rules"" message")]
		public void Then_I_should_see_create_rules_message()
		{
			writer.ToString().ShouldContain("create Rules");
		}
		
		[Then(@"I should see ""create layouts/default\.html"" message")]
		public void Then_I_should_see_create_layouts_default_html_message()
		{
			writer.ToString().ShouldContain(string.Format("create {0}", Path.Combine("layouts", "default.html")));
		}
		
		[Then(@"I should see ""create content/index\.html"" message")]
		public void Then_I_should_see_create_content_index_html_message()
		{
			writer.ToString().ShouldContain(string.Format("create {0}", Path.Combine("content", "index.html")));
		}
		
		[Then(@"I should see ""create content/stylesheet\.css"" message")]
		public void Then_I_should_see_create_content_stylesheet_css_message()
		{
			writer.ToString().ShouldContain(string.Format("create {0}", Path.Combine("content", "stylesheet.css")));
		}
		
		[Then(@"I should see ""Created a blank mulder site at '\[path]'\. Enjoy!"" message")]
		public void Then_I_should_see_created_a_blank_mulder_site_at_path_enjoy_message()
		{
			writer.ToString().ShouldContain(string.Format("Created a blank mulder site at '{0}'. Enjoy!", validPath));
		}
		
		[Then(@"I should have a directory created for my site")]
		public void Then_I_should_see_a_directory_created_for_my_site()
		{
			Directory.Exists(validPath).ShouldBe(true);
		}
		
		[Then(@"I should have my site directory populated with the default bare-bones site")]
		public void Then_I_should_have_my_site_directory_populated_with_the_default_bare_bones_site()
		{
			File.Exists(Path.Combine(validPath, "config.yaml")).ShouldBe(true);
			File.Exists(Path.Combine(validPath, "Rules")).ShouldBe(true);
			
			Directory.Exists(Path.Combine(validPath, "layouts")).ShouldBe(true);
			File.Exists(Path.Combine(validPath, "layouts", "default.html")).ShouldBe(true);
			
			Directory.Exists(Path.Combine(validPath, "content")).ShouldBe(true);
			File.Exists(Path.Combine(validPath, "content", "index.html")).ShouldBe(true);
			File.Exists(Path.Combine(validPath, "content", "stylesheet.css")).ShouldBe(true);
			
			Directory.Exists(Path.Combine(validPath, "lib")).ShouldBe(true);
		}
		
		[Then(@"the ""config\.yaml"" file should contain the default config")]
		public void Then_the_config_yaml_file_should_contain_the_default_config()
		{
			string expectedDefaultConfigContent = regex.Replace(File.ReadAllText(Path.Combine(tempTestPath, "config.yaml")), "");
			string actualDefaultConfigContent = regex.Replace(File.ReadAllText(Path.Combine(validPath, "config.yaml")), "");
			
			actualDefaultConfigContent.ShouldBe(expectedDefaultConfigContent);
		}
		
		[Then(@"the ""Rules"" file should contain the default rules")]
		public void Then_the_rules_file_should_contain_the_default_rules()
		{
			string expectedDefaultRulesContent = regex.Replace(File.ReadAllText(Path.Combine(tempTestPath, "Rules")), "");
			string actualDefaultRulesContent = regex.Replace(File.ReadAllText(Path.Combine(validPath, "Rules")), "");
			
			actualDefaultRulesContent.ShouldBe(expectedDefaultRulesContent);
		}
		
		[Then(@"the ""layouts/default\.html"" file should contain the default layout")]
		public void Then_the_layouts_default_html_file_should_contain_the_default_layout()
		{
			string expectedDefaultLayoutHtml = regex.Replace(File.ReadAllText(Path.Combine(tempTestPath, "default.html")), "");
			string actualDefaultLayoutHtml = regex.Replace(File.ReadAllText(Path.Combine(validPath, "layouts", "default.html")), "");
			
			actualDefaultLayoutHtml.ShouldBe(expectedDefaultLayoutHtml);
		}
		
		[Then(@"the ""content/index\.html"" file should contain the default content")]
		public void Then_the_content_index_html_file_should_contain_the_default_content()
		{
			string expectedDefaultHomePageHtml = regex.Replace(File.ReadAllText(Path.Combine(tempTestPath, "index.html")), "");
			string actualDefaultHomePageHtml = regex.Replace(File.ReadAllText(Path.Combine(validPath, "content", "index.html")), "");
			
			actualDefaultHomePageHtml.ShouldBe(expectedDefaultHomePageHtml);
		}
		
		[Then(@"the ""content/stylesheet\.css"" file should contain the default styles")]
		public void Then_the_content_stylesheet_css_file_should_contain_the_default_styles()
		{
			string expectedDefaultStylesheetCss = regex.Replace(File.ReadAllText(Path.Combine(tempTestPath, "stylesheet.css")), "");
			string actualDefaultStylesheetCss = regex.Replace(File.ReadAllText(Path.Combine(validPath, "content", "stylesheet.css")), "");
			
			actualDefaultStylesheetCss.ShouldBe(expectedDefaultStylesheetCss);
		}
		
		[Then(@"mulder should terminate with an success exit code")]
		public void Then_mulder_should_terminate_with_an_success_exit_code()
		{
			exitCode.ShouldBe(ExitCode.Success);
		}
		
		Stream GetResourceStreamFromResourceName(string resourceName)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		}
		
		void CreateFileFromResourceName(string resourceName, string path)
		{
			var fileSystem = new FileSystem();
			using (var resourceStream = GetResourceStreamFromResourceName(resourceName)) {
				fileSystem.WriteStreamToFile(path, resourceStream);
			}
		}
	}
}
