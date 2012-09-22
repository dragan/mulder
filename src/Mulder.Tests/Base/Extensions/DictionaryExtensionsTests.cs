using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Extensions;

namespace Mulder.Tests.Base.Extensions
{
	public class DictionaryExtensionsTests
	{
		[TestFixture]
		public class when_converting_dictionary_to_expando_object
		{
			[Test]
			public void should_contain_dictionary_items_and_values()
			{
				var dictionary = new Dictionary<string, object> {
					{ "item_one", 1 },
					{ "item_two", "two" }
				};

				dynamic expando = dictionary.ToExpando();
				Assert.That(expando.item_one == 1);
				Assert.That(expando.item_two == "two");
			}

			[Test]
			public void should_be_able_to_handle_a_dictionary_as_a_value()
			{
				var dictionary = new Dictionary<string, object> {
					{ "item_one", 1 },
					{ "item_two", "two" },
					{
						"item_three", 
						new Dictionary<string, object> {
							{ "item_one", 1 },
							{ "item_two", "two" }
						}
					}
				};

				dynamic expando = dictionary.ToExpando();
				Assert.That(expando.item_one == 1);
				Assert.That(expando.item_two == "two");
				Assert.That(expando.item_three.item_one == 1);
				Assert.That(expando.item_three.item_two == "two");
			}

			[Test]
			public void should_be_able_handle_a_collection_as_a_value()
			{
				var dictionary = new Dictionary<string, object> {
					{
						"item_one", 
						new List<string> { "one", "two", "three" }
					}
				};

				dynamic expando = dictionary.ToExpando();
				Assert.That(expando.item_one.Contains("one"));
				Assert.That(expando.item_one.Contains("two"));
				Assert.That(expando.item_one.Contains("three"));
			}
		}
	}
}
