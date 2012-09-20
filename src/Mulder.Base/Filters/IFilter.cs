using System;

namespace Mulder.Base.Filters
{
	public interface IFilter
	{
		string Execute(string source, dynamic model);
	}
}
