using System;

namespace Mulder.Base.Domain
{
	public class CompileRule : Rule, IEquatable<CompileRule>
	{
		readonly Action<RuleContext> block;
		
		public CompileRule(string identifierPattern, Action<RuleContext> block) : base(identifierPattern)
		{
			this.block = block;
		}
		
		public void ApplyTo(StaticFile staticFile, Site site)
		{
			var ruleContext = new RuleContext(staticFile, site);
			
			block.Invoke(ruleContext);
		}
		
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + block.GetHashCode();
			return hash + base.GetHashCode();
		}
		
		public override bool Equals(object obj)
		{
			if (obj is CompileRule) return Equals((CompileRule)obj);
			
			return false;
		}
		
		public bool Equals(CompileRule other)
		{
			if (other == null)
				return false;
			
			return block == other.block && base.Equals(other);
		}
	}
}
