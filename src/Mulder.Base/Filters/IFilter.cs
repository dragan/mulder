using System;

using Mulder.Base.Compilation;

namespace Mulder.Base.Filters
{
	public interface IFilter
	{
		string Execute(string source, FilterContext filterContext);
	}
}
