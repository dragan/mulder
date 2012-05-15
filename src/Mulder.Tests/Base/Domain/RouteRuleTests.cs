using System;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base.Domain;

namespace Mulder.Tests.Base.Domain
{
	public class RouteRuleTests
	{
		[TestFixture]
		public class when_applying_route_rule_to_static_file
		{
			string expected;
			StaticFile staticFile;
			Site site;
			RouteRule routeRule;
			
			[SetUp]
			public void SetUp()
			{
				expected = "Trust No One";
				staticFile = new StaticFile(CreateFakeItem());
				site = CreateFakeSite();
				
				routeRule = new RouteRule("*", (ruleContext) => {
					return expected;
				});
			}
			
			[Test]
			public void should_invoke_route_rule_action_block()
			{	
				string actual = routeRule.ApplyTo(staticFile, site);
				
				actual.ShouldBe(expected);
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
