using System;

namespace Mulder.Base.Domain
{
	public class LayoutRule : Rule, IEquatable<LayoutRule>
	{
		readonly string filterName;
		
		public string FilterName { get { return filterName; } }
		
		public LayoutRule(string identifierPattern, string filterName) : base(identifierPattern)
		{
			this.filterName = filterName;
		}
		
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + filterName.GetHashCode();
			return hash + base.GetHashCode();
		}
		
		public override bool Equals(object obj)
		{
			if (obj is LayoutRule) return Equals((LayoutRule)obj);
			
			return false;
		}
		
		public bool Equals(LayoutRule other)
		{
			if (other == null)
				return false;
			
			return filterName == other.filterName && base.Equals(other);
		}
	}
}
