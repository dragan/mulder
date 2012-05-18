using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base.Domain;

namespace Mulder.Tests.Base.Domain
{
	public class SiteTests
	{
		[TestFixture]
		public class when_creating_new_site
		{
			DateTime modificationTime;
			Site site;
			
			[SetUp]
			public void SetUp()
			{
				modificationTime = DateTime.UtcNow;
				
				var configuration = Substitute.For<IDictionary<string, object>>();
				var items = CreateItems(modificationTime);
				var layouts = Substitute.For<IEnumerable<Layout>>();
				var compileRules = Substitute.For<IEnumerable<CompileRule>>();
				var routeRules = Substitute.For<IEnumerable<RouteRule>>();
				var layoutRules = Substitute.For<IEnumerable<LayoutRule>>();
				
				site = new Site(configuration, items, layouts, compileRules, routeRules, layoutRules);
			}
			
			[Test]
			public void should_create_parent_and_child_links_on_each_item()
			{
				var actualItems = site.Items.ToList();
				var expectedItems = CreateExpectedItems();
				
				for (int i = 0; i < actualItems.Count; i++) {
					var actualItem = actualItems[i];
					var expectedItem = expectedItems[i];
					
					actualItem.Parent.ShouldBe(expectedItem.Parent);
					actualItem.Children.ShouldBe(expectedItem.Children);
				}
			}
			
			IEnumerable<Item> CreateItems(DateTime modificationTime)
			{
				var content = "";
				var meta = Substitute.For<IDictionary<string, object>>();
				
				return new List<Item> {
					new Item("/",
						false,
						content,
						meta,
						modificationTime),
					new Item("/a/",
						false,
						content,
						meta,
						modificationTime),
					new Item("/a/b/",
						false,
						content,
						meta,
						modificationTime),
					new Item("/a/b/c/",
						false,
						content,
						meta,
						modificationTime),
					new Item("/b/",
						false,
						content,
						meta,
						modificationTime),
					new Item("/b/a/",
						false,
						content,
						meta,
						modificationTime),
					new Item("/b/a/b/",
						false,
						content,
						meta,
						modificationTime),
					new Item("/b/a/b/c/",
						false,
						content,
						meta,
						modificationTime),
					new Item("/binary/",
						true,
						content,
						meta,
						modificationTime)
				};
			}
			
			List<Item> CreateExpectedItems()
			{
				var items = CreateItems(modificationTime).ToList();
				
				var root = items[0];
				var a = items[1];
				var ab = items[2];
				var abc = items[3];
				var b = items[4];
				var ba = items[5];
				var bab = items[6];
				var babc = items[7];
				var binary = items[8];
				
				root.Parent = null;
				root.AddChild(a);
				root.AddChild(b);
				root.AddChild(binary);
				
				a.Parent = root;
				a.AddChild(ab);
				ab.Parent = a;
				ab.AddChild(abc);
				abc.Parent = ab;
				
				b.Parent = root;
				b.AddChild(ba);
				ba.Parent = b;
				ba.AddChild(bab);
				bab.Parent = ba;
				bab.AddChild(babc);
				babc.Parent = bab;
				
				binary.Parent = root;
				
				return items;
			}
		}
	}
}
