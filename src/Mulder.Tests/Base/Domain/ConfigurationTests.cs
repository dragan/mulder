using System;
using System.Collections.Generic;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Domain;

namespace Mulder.Tests.Base.Domain
{
	public class ConfigurationTests
	{
		[TestFixture]
		public class when_instantiating_with_another_dictionary
		{
			[Test]
			public void should_contain_same_keys_and_values()
			{
				var existingDictionary = new Dictionary<string, object> {
					{ "Key1", "1" },
					{ "Key2", "2" },
					{ "Key3", 3 },
					{ "Key4", "Four" }
				};

				var actual = new Configuration(existingDictionary);

				foreach (var kvp in existingDictionary) {
					actual.Contains(kvp).ShouldBe(true);
				}
			}
		}
	}
}
