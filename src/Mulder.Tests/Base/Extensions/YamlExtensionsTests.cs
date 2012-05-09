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
	}
}
