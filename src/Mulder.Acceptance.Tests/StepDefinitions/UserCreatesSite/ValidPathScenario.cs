using System;
using System.IO;

using TechTalk.SpecFlow;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCreatesSite
{
	[Binding]
    public class ValidPathScenario
	{
		[Given(@"I have a valid path")]
		public void GivenIHaveAValidPath()
		{
			ScenarioContext.Current.Pending();
		}

		[When(@"I run the create site command with a valid path")]
		public void WhenIRunTheCreateSiteCommandWithValidPath()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""create config\.yaml"" message")]
		public void ThenIShouldSeeCreateConfig_YamlOnScreen()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""create Rules"" message")]
		public void ThenIShouldSeeCreateRulesOnScreen()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""create layouts/default\.html"" message")]
		public void ThenIShouldSeeCreateLayoutsDefault_HtmlOnScreen()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""create content/index\.html"" message")]
		public void ThenIShouldSeeCreateContentIndex_HtmlOnScreen()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""create content/stylesheet\.css"" message")]
		public void ThenIShouldSeeCreateContentStylesheet_CssOnScreen()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""Created a blank mulder site at '\[path]'\. Enjoy!"" message")]
		public void ThenIShouldSeeCreatedABlankMulderSiteAtValidPath_EnjoyOnScreen()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should have a directory created for my site")]
		public void ThenIShouldSeeADirectoryCreatedForMySite()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should have my site directory populated with the default bare-bones site")]
		public void ThenIShouldHaveMySiteDirectoryPopulatedWithTheDefaultBare_BonesSite()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the config\.yaml file should contain the default config")]
		public void ThenTheConfig_YamlFileShouldContainTheDefaultConfig()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the Rules file should contain the default Rules")]
		public void ThenTheRulesFileShouldContainTheDefaultRules()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the default\.html file should contain the default layout")]
		public void ThenTheDefault_HtmlFileShouldContainTheDefaultLayout()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the index\.html file should contain the default content")]
		public void ThenTheIndex_HtmlFileShouldContainTheDefaultContent()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"the stylesheet\.css file should contain the default styles")]
		public void ThenTehStylesheet_CssFileShouldContainTheDefaultStyles()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"mulder should terminate successfully")]
		public void ThenMulderShouldTerminateSuccessfully()
		{
			ScenarioContext.Current.Pending();
		}
	}
}
