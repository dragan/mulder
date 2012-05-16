using System;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Compilation;
using Mulder.Base.Filters;

namespace Mulder.Tests.Base.Compilation
{
	public class FilterFactoryTests
	{
		[TestFixture]
		public class when_creating_filters
		{
			[Test]
			public void should_be_able_to_create_liquid_filter()
			{
				var filterFactory = new FilterFactory();
				IFilter filter = filterFactory.CreateFilter("liquid");
				
				filter.ShouldBeTypeOf(typeof(LiquidFilter));
			}
		}
	}
}

