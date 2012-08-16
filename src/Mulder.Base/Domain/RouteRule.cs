using System;

namespace Mulder.Base.Domain
{
	public class RouteRule : Rule, IEquatable<RouteRule>
	{
		readonly Func<RuleContext, string> block;
		
		public RouteRule(string identifierPattern, Func<RuleContext, string> block) : base(identifierPattern)
		{
			this.block = block;
		}
		
		public string ApplyTo(StaticFile staticFile, Site site)
		{
			var ruleContext = new RuleContext(staticFile, site);
			
			return block.Invoke(ruleContext);
		}
		
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + block.GetHashCode();
			return hash + base.GetHashCode();
		}
		
		public override bool Equals(object obj)
		{
			if (obj is RouteRule) return Equals((RouteRule)obj);
			
			return false;
		}
		
		public bool Equals(RouteRule other)
		{
			if (other == null)
				return false;
			
			return block == other.block && base.Equals(other);
		}
	}
}
