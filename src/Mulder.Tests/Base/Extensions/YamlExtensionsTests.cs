using System;
using System.Collections.Generic;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Extensions;

namespace Mulder.Tests.Base.Extensions
{
	public class YamlExtensionsTests
	{
		[TestFixture]
		public class when_serializing_dictionary_containing_only_strings_to_yaml
		{
			IDictionary<string, object> dictionary;
			string expectedYaml;
			
			[SetUp]
			public void Init()
			{
				dictionary = new Dictionary<string, object> {
					{ "Property1", "1" },
					{ "Property2", "2" },
					{ "Property3", "3" }
				};
			
				expectedYaml = string.Format("property1: 1{0}property2: 2{0}property3: 3{0}", Environment.NewLine);
			}
			
			[Test]
			public void should_create_yaml_string()
			{
				string yaml = dictionary.ToYaml();
			
				yaml.ShouldBe(expectedYaml);
			}
		}
		
		[TestFixture]
		public class when_serializing_dictionary_containing_only_strings_to_yaml_header
		{
			IDictionary<string, object> dictionary;
			string expectedHeader;
			
			[SetUp]
			public void Init()
			{
				dictionary = new Dictionary<string, object> {
					{ "Property1", "1" },
					{ "Property2", "2" },
					{ "Property3", "3" }
				};
			
				expectedHeader = string.Format("---{0}property1: 1{0}property2: 2{0}property3: 3{0}---{0}", Environment.NewLine);
			}
			
			[Test]
			public void should_create_a_yaml_header_string()
			{
				string yaml = dictionary.ToYamlHeader();
			
				yaml.ShouldBe(expectedHeader);
			}
		}
		
		[TestFixture]
		public class when_deserializing_an_empty_string
		{
			[Test]
			public void should_return_empty_dictionary()
			{
				"".DeserializeYaml().ShouldBeEmpty();
			}
		}
		
		[TestFixture]
		public class when_deserializing_yaml_text
		{
			string text = @"---
title: X-Files
cast:
  -
    name: David Duchovny
    role: Fox Mulder
  -
    name: Gillian Anderson
    role: Dana Scully
...";
			
			IDictionary<string, object> result;
			
			[SetUp]
			public void Init()
			{
				result = text.DeserializeYaml()[0] as IDictionary<string, object>;
			}
			
			[Test]
			public void should_return_populated_dictionary()
			{
				result.ShouldNotBeEmpty();
			}
			
			[Test]
			public void should_contain_title()
			{
				result.ShouldContainKeyAndValue("title", "X-Files");
			}
			
			[Test]
			public void should_contain_two_cast_members()
			{
				var cast = result["cast"] as List<object>;
				cast.Count.ShouldBe(2);
			}
			
			[Test]
			public void should_have_david_duchovny_as_first_cast_member()
			{
				var cast = result["cast"] as List<object>;
				var member = cast[0] as IDictionary<string, object>;
				member["name"].ShouldBe("David Duchovny");
				member["role"].ShouldBe("Fox Mulder");
			}
			
			[Test]
			public void should_have_gillian_anderson_as_second_cast_member()
			{
				var cast = result["cast"] as List<object>;
				var member = cast[1] as IDictionary<string, object>;
				member["name"].ShouldBe("Gillian Anderson");
				member["role"].ShouldBe("Dana Scully");
			}
		}
		
		[TestFixture]
		public class when_deserializing_a_yaml_header
		{
			string textWithoutHeader = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
			
			string textWithHeader = @"---
Property1: 1
Property2: 2
Property3: 3
---

Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
			
			[Test]
			public void should_return_empty_dictionary_if_header_is_not_present()
			{
				textWithoutHeader.DeserializeYamlHeader().ShouldBeEmpty();
			}
			
			[Test]
			public void should_return_dictionary_of_items_if_header_is_present()
			{
				IDictionary<string, object> items = textWithHeader.DeserializeYamlHeader();
				
				items.Count.ShouldBe(3);
				items.ShouldContainKeyAndValue("Property1", "1");
				items.ShouldContainKeyAndValue("Property2", "2");
				items.ShouldContainKeyAndValue("Property3", "3");
			}
		}
		
		[TestFixture]
		public class when_excluding_the_yaml_header
		{
			string textWithoutHeader = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
			
			string textWithHeader = @"---
Property1: 1
Property2: 2
Property3: 3
---

Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
			
			[Test]
			public void should_return_text_if_header_is_not_present()
			{
				textWithoutHeader.ExcludeYamlHeader().ShouldBe(textWithoutHeader);
			}
			
			[Test]
			public void should_return_text_witout_header_if_header_is_present()
			{
				textWithHeader.ExcludeYamlHeader().ShouldBe(textWithoutHeader);
			}
		}
	}
}
