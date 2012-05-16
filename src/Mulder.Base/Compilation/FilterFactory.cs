using System;

using Mulder.Base.Domain;
using Mulder.Base.Filters;

namespace Mulder.Base.Compilation
{
	public class FilterFactory : IFilterFactory
	{
		public FilterFactory()
		{
		}
		
		public IFilter CreateFilter(string filterName)
		{
			Filters filter = (Filters)Enum.Parse(typeof(Filters), filterName.ToUpper());
			
			switch (filter) {
			case Filters.LIQUID:
				return new LiquidFilter();
			case Filters.MARKDOWN:
				return new MarkdownFilter();
			default:
				return null;
			}
		}
	}
	
	public interface IFilterFactory
	{
		IFilter CreateFilter(string filterName);
	}
}
