using System;
using System.Linq;

namespace Mulder.Base.Domain
{
	public class RuleContext : IEquatable<RuleContext>
	{
		readonly StaticFile staticFile;
		readonly Item item;
		readonly Site site;
		
		public StaticFile StaticFile { get { return staticFile; } }
		public Item Item { get { return item; } }
		public Site Site { get { return site; } }
		
		public RuleContext(StaticFile staticFile, Site site)
		{
			this.staticFile = staticFile;
			this.item = staticFile.Item;
			this.site = site;
		}
		
		public RuleContext WithFilter(string filterName)
		{
			staticFile.AddFilter(filterName);
			
			return this;
		}
		
		public RuleContext WithLayout(string layoutName)
		{
			string layoutIdentifier = layoutName;
			layoutIdentifier = layoutIdentifier.StartsWith("/") ? layoutIdentifier : "/" + layoutIdentifier;
			layoutIdentifier = layoutIdentifier.EndsWith("/") ? layoutIdentifier : layoutIdentifier + "/";
			
			var layout = site.GetLayoutWithIdentifier(layoutIdentifier);
			
			staticFile.Layout = layout;
			
			return this;
		}
		
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + staticFile.GetHashCode();
			hash = hash * 23 + item.GetHashCode();
			hash = hash * 23 + site.GetHashCode();
			return hash;
		}
		
		public override bool Equals(object obj)
		{
			if (obj is RuleContext) return Equals((RuleContext)obj);
			
			return false;
		}
		
		public bool Equals(RuleContext other)
		{
			if (other == null)
				return false;
			
			return staticFile == other.staticFile
				&& item == other.item
				&& site == other.site;
		}
	}
}
