using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Domain;

namespace Mulder.Tests.Base.Domain
{
	public class RuleTests
	{
		[TestFixture]
		public class when_checking_if_rule_is_applicable_to_item
		{
			[Test]
			public void should_be_applicable_with_just_string_for_pattern()
			{
				var applicableItem = CreateItem("/a/");
				
				var rule = new Rule("a");
				
				rule.ApplicableTo(applicableItem).ShouldBe(true);
			}

			[Test]
			public void should_all_be_applicable_with_just_a_star_wild_card_for_pattern()
			{
				var applicableItems = new List<Item> {
					CreateItem("/a/"),
					CreateItem("/a/b/"),
					CreateItem("/a/b/c/"),
					CreateItem("/a/b/c/d/"),
					CreateItem("/a/b/c/d/e/")
				};
				
				var rule = new Rule("*");
				
				applicableItems.ForEach(item => rule.ApplicableTo(item).ShouldBe(true));
			}
			
			[Test]
			public void should_be_applicable_with_star_wild_card_and_specific_directory_for_pattern()
			{
				var applicableItems = new List<Item> {
					CreateItem("/a/b/c/"),
					CreateItem("/a/foo/c/")
				};
				
				var rule = new Rule("a/*/c");
				
				applicableItems.ForEach(item => rule.ApplicableTo(item).ShouldBe(true));
			}
			
			[Test]
			public void should_not_be_applicable_for_star_wild_card_and_specific_directory_for_pattern()
			{
				var nonApplicableItems = new List<Item> {
					CreateItem("/a/"),
					CreateItem("/a/b/"),
					CreateItem("/a/b/c/d/"),
					CreateItem("/a/b/c/d/e/")
				};
				
				var rule = new Rule("a/*/c");
				
				nonApplicableItems.ForEach(item => rule.ApplicableTo(item).ShouldBe(false));
			}
			
			[Test]
			public void should_all_be_applicable_for_plus_wild_card_with_string()
			{
				var applicableItems = new List<Item> {
					CreateItem("/foobar/images/bar/"),
					CreateItem("/foosoap/scripts/bar/"),
					CreateItem("/fooreallylongfoo/")
				};
				
				var rule = new Rule("foo+");
				
				applicableItems.ForEach(item => rule.ApplicableTo(item).ShouldBe(true));
			}
			
			[Test]
			public void should_not_be_applicable_for_plus_wild_card_with_string()
			{
				var nonApplicableItems = new List<Item> {
					CreateItem("/barfoo/"),
					CreateItem("/barfoos/"),
					CreateItem("/reallylongbar/foos/")
				};
				
				var rule = new Rule("foo+");
				
				nonApplicableItems.ForEach(item => rule.ApplicableTo(item).ShouldBe(false));
			}
			
			[Test]
			public void should_be_able_to_use_star_and_plus_wild_card_together()
			{
				var item = CreateItem("/a/b/c/foobar/");
				
				var rule = new Rule("a/*/c/foo+");
				
				rule.ApplicableTo(item).ShouldBe(true);
			}
			
			static Item CreateItem(string identifier)
			{
				return new Item(identifier, false, "", new Dictionary<string, object>(), DateTime.Now);
			}
		}
	}
}
