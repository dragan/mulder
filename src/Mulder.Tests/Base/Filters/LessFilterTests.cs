using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base.Compilation;
using Mulder.Base.Filters;
using Mulder.Base.IO;

namespace Mulder.Tests.Base.Filters
{
	public class LessFilterTests
	{
		[TestFixture]
		public class when_executing_less_filter
		{
			[Test]
			public void should_be_able_to_transform_source()
			{
				string source = "@color: #4D926F;#header {color: @color;}h2 {color: @color;}";
				string expected = "#header{color:#4d926f}h2{color:#4d926f}";

				var filterContext = new FilterContext {
					Item = new Dictionary<string, object> {
						{ "filename", Path.Combine("a", "b", "c.less") }
					}
				};
				
				var fileSystem = Substitute.For<IFileSystem>();
				
				var lessFilter = new LessFilter(fileSystem);
				
				string result = lessFilter.Execute(source, filterContext);
				
				result.ShouldBe(expected);
			}
		}
		
		[TestFixture]
		public class when_executing_less_filter_on_a_source_that_has_a_nested_dependency
		{
			string dependencyFilename;
			string source;
			string expected;
			FilterContext filterContext;
			IFileSystem fileSystem;
			LessFilter lessFilter;
			
			[SetUp]
			public void SetUp()
			{
				string nestedDependencyFilename = Path.Combine("a", "b", "variables.less");
				string nestedDependencyText = "@import \"dependency.less\";";
				
				dependencyFilename = Path.Combine("a", "b", "dependency.less");
				var dependencyText = "@color: #4D926F;";
				
				source = "@import \"variables.less\";#header {color: @color;}h2 {color: @color;}";
				expected = "#header{color:#4d926f}h2{color:#4d926f}";
				
				filterContext = new FilterContext {
					Item = new Dictionary<string, object> {
						{ "filename", Path.Combine("a", "b", "c.less") }
					}
				};
				
				fileSystem = Substitute.For<IFileSystem>();
				fileSystem.FileExists(nestedDependencyFilename).Returns(true);
				fileSystem.ReadStringFromFile(nestedDependencyFilename).Returns(nestedDependencyText);
				fileSystem.FileExists(dependencyFilename).Returns(true);
				fileSystem.ReadStringFromFile(dependencyFilename).Returns(dependencyText);
				
				lessFilter = new LessFilter(fileSystem);
			}
			
			[Test]
			public void should_call_file_exists_on_file_system()
			{
				lessFilter.Execute(source, filterContext);
				
				fileSystem.Received().FileExists(dependencyFilename);
			}
			
			[Test]
			public void should_call_read_string_from_file_on_file_system()
			{
				lessFilter.Execute(source, filterContext);
				
				fileSystem.Received().ReadStringFromFile(dependencyFilename);
			}
			
			[Test]
			public void should_receive_expected_transformed_result()
			{
				string result = lessFilter.Execute(source, filterContext);
				
				result.ShouldBe(expected);
			}
		}
	}
}
