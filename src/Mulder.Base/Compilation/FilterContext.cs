using System;

using Mulder.Base.Domain;

namespace Mulder.Base.Compilation
{
	public class FilterContext
	{
		public IConfiguration Configuration { get; set; }
		public dynamic Layout { get; set; }
		public dynamic Item  { get; set; }
		public string Content { get; set; }
	}
}
