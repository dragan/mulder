using System;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base.Domain;

namespace Mulder.Tests.Base.Domain
{
	public class CompileRuleTests
	{
		[TestFixture]
		public class when_applying_compile_rule_to_static_file
		{
			bool actionInvoked;
			StaticFile staticFile;
			Site site;
			CompileRule compileRule;
			
			[SetUp]
			public void SetUp()
			{
				actionInvoked = false;
				staticFile = new StaticFile(CreateFakeItem());
				site = CreateFakeSite();
				
				compileRule = new CompileRule("*", (ruleContext) => {
					actionInvoked = true;
				});
			}
			
			[Test]
			public void should_invoke_route_rule_action_block()
			{	
				compileRule.ApplyTo(staticFile, site);
				
				actionInvoked.ShouldBe(true);
			}
			
			static Item CreateFakeItem()
			{
				return new Item("", false, "", new Dictionary<string, object>(), DateTime.Now);
			}
			
			static Site CreateFakeSite()
			{
				return new Site(
					Substitute.For<IDictionary<string, object>>(),
					Substitute.For<IList<Item>>(),
					Substitute.For<IList<Layout>>(),
					Substitute.For<IList<CompileRule>>(),
					Substitute.For<IList<RouteRule>>(),
					Substitute.For<IList<LayoutRule>>());
			}
		}
	}
}
