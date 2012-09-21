using System;

using Mulder.Base.Filters;
using Mulder.Base.IO;

namespace Mulder.Base.Compilation
{
	public class FilterFactory : IFilterFactory
	{
		readonly IFileSystem fileSystem;
		
		public FilterFactory(IFileSystem fileSystem)
		{
			this.fileSystem = fileSystem;
		}
		
		public IFilter CreateFilter(string filterName)
		{
			Filters filter = (Filters)Enum.Parse(typeof(Filters), filterName.ToUpper());
			
			switch (filter) {
			case Filters.LIQUID:
				return new LiquidFilter();
			case Filters.MARKDOWN:
				return new MarkdownFilter();
			case Filters.LESS:
				return new LessFilter(fileSystem);
			case Filters.RAZOR:
				return new RazorFilter();
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
