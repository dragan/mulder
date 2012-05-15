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
			return null;
		}
	}
	
	public interface IFilterFactory
	{
		IFilter CreateFilter(string filterName);
	}
}
