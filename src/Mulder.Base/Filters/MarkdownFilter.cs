using System;

using MarkdownSharp;

namespace Mulder.Base.Filters
{
	public class MarkdownFilter : IFilter
	{
		public MarkdownFilter()
		{
		}
		
		public string Execute(string source, dynamic model)
		{
			var markdown = new Markdown();
			string output = markdown.Transform(source);
			
			return output;
		}
	}
}
