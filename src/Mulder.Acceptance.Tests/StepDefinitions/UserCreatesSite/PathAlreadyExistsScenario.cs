using System;

using TechTalk.SpecFlow;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCreatesSite
{
	[Binding]
    public class PathAlreadyExistsScenario
	{
		[Given(@"I have a path that already exists")]
		public void GivenIHaveAPathThatAlreadyExists()
		{
			ScenarioContext.Current.Pending();
		}
		
		[When(@"I run the create site command with a path that already exists")]
		public void WhenIRunTheCreateSiteCommandWithPathThatAlreadyExists()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see ""A site at '\[path]' already exists\."" message")]
		public void ThenIShouldSeeASiteAtPathAlreadyExists_Message()
		{
			ScenarioContext.Current.Pending();
		}
	}
}
