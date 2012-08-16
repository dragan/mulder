using System;
using System.Collections.Generic;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Extensions;

namespace Mulder.Tests.Base.Extensions
{
	public class AnonymousTypeExtensionsTests
	{
		[TestFixture]
		public class when_converting_anonymous_type_to_dictionary
		{
			[Test]
			public void should_create_return_a_dictionary()
			{
				var anonymousType = new {};
				
				anonymousType.ToDictionary().ShouldBeTypeOf(typeof(IDictionary<string, object>));
			}
			
			[Test]
			public void should_return_dictionary_containing_types_properties()
			{
				var anonymousType = new { Jelly = "Donut", Strings = "Guitar", Watson = "Holmes", Simon = "Garfunkel" };
				
				anonymousType.ToDictionary().ShouldContainKeyAndValue("Jelly", "Donut");
				anonymousType.ToDictionary().ShouldContainKeyAndValue("Strings", "Guitar");
				anonymousType.ToDictionary().ShouldContainKeyAndValue("Watson", "Holmes");
				anonymousType.ToDictionary().ShouldContainKeyAndValue("Simon", "Garfunkel");
			}
		}
	}
}
