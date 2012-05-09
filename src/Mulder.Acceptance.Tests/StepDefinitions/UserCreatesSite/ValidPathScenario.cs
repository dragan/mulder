using System;
using System.IO;

using Shouldly;
using TechTalk.SpecFlow;

using Mulder.Base;
using Mulder.Base.Logging;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCreatesSite
{
	[Binding]
    public class ValidPathScenario
	{
		string validPath;
		StringWriter writer;
		EntryPoint entryPoint;
		ExitCode exitCode;
		
		[BeforeStep]
		public void BeforeStep()
		{
			writer = new StringWriter();
			entryPoint = new EntryPoint(new Log(writer, new LogLevel[] { LogLevel.Error }));
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
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""create Rules"" message")]
		public void Then_I_should_see_create_rules_message()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""create layouts/default\.html"" message")]
		public void Then_I_should_see_create_layouts_default_html_message()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""create content/index\.html"" message")]
		public void Then_I_should_see_create_content_index_html_message()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""create content/stylesheet\.css"" message")]
		public void Then_I_should_see_create_content_stylesheet_css_message()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""Created a blank mulder site at '\[path]'\. Enjoy!"" message")]
		public void Then_I_should_see_created_a_blank_mulder_site_at_path_enjoy_message()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should have a directory created for my site")]
		public void Then_I_should_see_a_directory_created_for_my_site()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should have my site directory populated with the default bare-bones site")]
		public void Then_I_should_have_my_site_directory_populated_with_the_default_bare_bones_site()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the ""config\.yaml"" file should contain the default config")]
		public void Then_the_config_yaml_file_should_contain_the_default_config()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the ""Rules"" file should contain the default rules")]
		public void Then_the_rules_file_should_contain_the_default_rules()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the ""layouts/default\.html"" file should contain the default layout")]
		public void Then_the_layouts_default_html_file_should_contain_the_default_layout()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the ""content/index\.html"" file should contain the default content")]
		public void Then_the_content_index__html_file_should_contain_the_default_content()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the ""content/stylesheet\.css"" file should contain the default styles")]
		public void Then_the_content_stylesheet_css_file_should_contain_the_default_styles()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"mulder should terminate with an success exit code")]
		public void Then_mulder_should_terminate_with_an_success_exit_code()
		{
			ScenarioContext.Current.Pending();
		}
	}
}
