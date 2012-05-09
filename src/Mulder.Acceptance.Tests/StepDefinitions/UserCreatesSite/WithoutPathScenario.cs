using System;

using TechTalk.SpecFlow;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCreatesSite
{
	[Binding]
    public class WithoutPathScenario
	{
		[When(@"I run the create site command without a path")]
		public void WhenIRunTheCreateSiteCommandWithoutAPath()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""usage: create site \[path]"" message")]
		public void ThenIShouldSeeUsageCreateSitePathMessage()
		{
			ScenarioContext.Current.Pending();
		}
	}
}
		
